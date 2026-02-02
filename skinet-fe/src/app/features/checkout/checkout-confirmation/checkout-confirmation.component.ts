import { CurrencyPipe } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { CartService } from '../../../core/services/cart.service';
import { AddressPipe } from "../../../shared/pipes/address-pipe";
import { PaymentCardPipe } from "../../../shared/pipes/payment-pipe";

@Component({
  selector: 'app-checkout-confirmation',
  imports: [
    CurrencyPipe,
    AddressPipe,
    PaymentCardPipe
],
  templateUrl: './checkout-confirmation.component.html',
  styleUrl: './checkout-confirmation.component.scss'
})
export class CheckoutConfirmationComponent {
  cartService = inject(CartService);
  @Input() confirmationToken?: ConfirmationToken;
}
