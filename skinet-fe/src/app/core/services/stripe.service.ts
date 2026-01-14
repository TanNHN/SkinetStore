import { inject, Injectable } from '@angular/core';
import { firstValueFrom, map, Observable } from 'rxjs';
import { ConfirmationToken, loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements, StripePaymentElement } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { AccountService } from './account.service';
import { MatStepper } from '@angular/material/stepper';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  private stripePromise?: Promise<Stripe | null>;
  private cartService = inject(CartService);
  private accountService = inject(AccountService);
  private http = inject(HttpClient);
  private elements?: StripeElements;
  private addressElement?: StripeAddressElement;
  private paymentElement?: StripePaymentElement;

  
  constructor() {
    this.stripePromise = loadStripe(environment.stripePublicKey);
  }

  getStripe(): Promise<Stripe | null> | undefined {
    return this.stripePromise;
  }

  async getStripeElement() {
    if (!this.elements) {
      const stripe = await this.getStripe();
      if (!stripe) throw new Error('Stripe failed to load.');
      const cart = await firstValueFrom(this.createOrUpdatePaymentIntent());
      this.elements = stripe.elements({ clientSecret: cart.clientSecret, appearance: { labels: 'floating' } });
    }
    return this.elements;
  }

  async createAddressElement() {
    if (!this.addressElement) {
      const elements = await this.getStripeElement();
      if (elements) {
        const user = this.accountService.currentUser();
        let stripeAddress: StripeAddressElementOptions['defaultValues'] = {};
        if (user) {
          stripeAddress.name = user.firstName + ' ' + user.lastName;
        }
        if (user?.address) {
          stripeAddress.address = {
            line1: user.address.line1,
            line2: user.address.line2,
            country: user.address.country,
            city: user.address.city,
            postal_code: user.address.postalCode,
          }
        }

        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues: stripeAddress
        }
        this.addressElement = elements.create('address', options);
      } else {
        throw new Error('Stripe Elements not initialized.');
      }
    }
    return this.addressElement;
  }

  async createPaymentElement() {
    if (!this.paymentElement) {
      const elements = await this.getStripeElement();
      if (elements) {
        this.paymentElement = elements.create('payment');
      } else {
        throw new Error('Stripe Elements not initialized.')
      }
    }
    return this.paymentElement;
  }

  createOrUpdatePaymentIntent() {
    const cart = this.cartService.cart();
    if (!cart) throw new Error('Problem with Cart');
    return this.http.post<Cart>(`${environment.apiUrl}payment/${cart.id}`, {}).pipe(
      map(cart => {
        this.cartService.setCart(cart);
        //when calling this function we can set the cart and also get the cart data
        return cart;
      })
    );
  }

  async createPaymentConfirmationToken() {
    const stripe = await this.getStripe();
    const element = await this.getStripeElement();
    const result = await element.submit();
    if (result.error) throw new Error(result.error.message);
    if (stripe) {
      return await stripe.createConfirmationToken({
        elements: element
      });
    }
    else {
      throw new Error('Stripe failed to load.');
    }
  }

  disposeService() {
    this.elements = undefined;
    this.addressElement = undefined;
    this.paymentElement = undefined;
  }

  async confirmToken(confirmToken?: ConfirmationToken) {
    const stripe = await this.getStripe();
    const elements = await this.getStripeElement();
    const result = await elements.submit();
    if (result.error) throw new Error(result.error.message);

    const clientSecret = this.cartService.cart()?.clientSecret;
    if (stripe && clientSecret && confirmToken) {
      return await stripe.confirmPayment({
        clientSecret: clientSecret,
        confirmParams: {
          confirmation_token: confirmToken.id,
        },
        redirect: 'if_required'
      });
    } else {
      throw new Error('Stripe failed to load.')
    }
  }
}