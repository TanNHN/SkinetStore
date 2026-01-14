import { Component, inject, Input } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';
import { CurrencyPipe } from '@angular/common';
import { ConfirmationToken } from '@stripe/stripe-js';
import { AddressPipe } from "../../../shared/pipes/address-pipe";
import { PaymentPipe } from "../../../shared/pipes/payment-pipe";

@Component({
  selector: 'app-checkout-confirmation',
  imports: [
    CurrencyPipe,
    AddressPipe,
    PaymentPipe
],
  templateUrl: './checkout-confirmation.component.html',
  styleUrl: './checkout-confirmation.component.scss'
})
export class CheckoutConfirmationComponent {
  cartService = inject(CartService);
  @Input() confirmationToken?: ConfirmationToken;
}
