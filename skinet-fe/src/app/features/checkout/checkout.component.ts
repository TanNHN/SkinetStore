import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MatStepper, MatStepperModule } from '@angular/material/stepper';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { Router, RouterLink } from "@angular/router";
import { StripeService } from '../../core/services/stripe.service';
import { ConfirmationToken, StripeAddressElement, StripeAddressElementChangeEvent, StripePaymentElement, StripePaymentElementChangeEvent } from '@stripe/stripe-js';
import { SnackbarService } from '../../core/services/snackbar.service';
import { MatButton } from '@angular/material/button';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { CheckoutConfirmationComponent } from "./checkout-confirmation/checkout-confirmation.component";
import { CartService } from '../../core/services/cart.service';
import { CurrencyPipe, JsonPipe } from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-checkout',
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    RouterLink,
    MatButton,
    MatCheckboxModule,
    CheckoutDeliveryComponent,
    CheckoutConfirmationComponent,
    CurrencyPipe,
    JsonPipe,
    MatProgressSpinnerModule
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit, OnDestroy {
  addressElement?: StripeAddressElement;
  paymentElement?: StripePaymentElement;
  stripeService = inject(StripeService);
  private snackBar = inject(SnackbarService);
  cartService = inject(CartService);
  completionStatus = signal<{ address: boolean, card: boolean, delivery: boolean }>({
    address: false,
    card: false,
    delivery: false
  });
  isSaveAddress: boolean = false;
  private accountService = inject(AccountService);
  private routerService = inject(Router);
  confirmationToken = signal<ConfirmationToken | undefined>(undefined);
  isLoadding = signal(false);

  async ngOnInit() {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement.mount('#address-element');
      this.addressElement.on('change', this.handleAddressChange);

      this.paymentElement = await this.stripeService.createPaymentElement();
      this.paymentElement.mount('#payment-element');
      this.paymentElement.on('change', this.handlePaymentChange);

    } catch (error: any) {
      this.snackBar.error(error.message);
    }
  }

  async getConfirmationToken() {
    try {
      if (Object.values(this.completionStatus()).every(status => status === true)) {
        const result = await this.stripeService.createPaymentConfirmationToken();
        if (result.error) throw new Error(result.error.message);
        this.confirmationToken.set(result.confirmationToken);
      }
    } catch (error: any) {
      this.snackBar.error(error.message);
    }
  }

  onChkSaveAddressChange(event: MatCheckboxChange) {
    this.isSaveAddress = event.checked;
  }

  handleAddressChange = (event: StripeAddressElementChangeEvent) => {
    this.completionStatus.update(state => ({
      ...state,
      address: event.complete
    }))

  }

  handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.completionStatus.update(state => ({
      ...state,
      card: event.complete
    }))
  }

  handleDeliveryChange = (event: boolean) => {
    this.completionStatus.update(state => ({
      ...state,
      delivery: event
    }))
  }

  ngOnDestroy(): void {
    this.stripeService.disposeService();
  }

  async getAddressFromTripeAddress() {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;
    if (address) {
      return {
        line1: address.line1 || '',
        line2: address.line2 || '',
        city: address.city || '',
        state: address.state || '',
        postalCode: address.postal_code || '',
        country: address.country || ''
      }
    }
    return null;
  }

  async onStepChange(event: StepperSelectionEvent) {
    if (event.selectedIndex === 1) {
      if (this.isSaveAddress) {
        const address = await this.getAddressFromTripeAddress();
        address && firstValueFrom(this.accountService.updateAddress(address));
      }
    }
    if (event.selectedIndex === 2) {
      await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
    } if (event.selectedIndex === 3) {
      await this.getConfirmationToken();
    }
  }

  async confirmPayment(stepper: MatStepper) {
    this.isLoadding.set(true);
    try {
      if (this.confirmationToken()) {
        const result = await this.stripeService.confirmToken(this.confirmationToken());
        if (result.error) throw new Error(result.error.message);
        this.cartService.deleteCart();
        this.cartService.deliveryMethod.set(null);
        this.routerService.navigateByUrl('checkout-success');
      }
    } catch (error: any) {
      this.snackBar.error(error.message || 'Confirm payment error');
      stepper.previous();
    } finally {
      this.isLoadding.set(false);
    }
  }
}
