import { Component, inject, Input } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { StripeService } from '../../../core/services/stripe.service';

@Component({
  selector: 'app-checkout-success',
  imports: [],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent {
  private stripeService = inject(StripeService);

}
