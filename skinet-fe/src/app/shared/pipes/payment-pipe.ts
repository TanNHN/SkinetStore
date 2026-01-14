import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'payment'
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken['payment_method_preview'], ...args: unknown[]): string {
    if (value?.card) {
      const { display_brand, exp_month, exp_year, last4 } = value.card
      return `${display_brand?.toUpperCase()} **** **** **** ${last4}, ${exp_month}/${exp_year}`
    }
    return 'Unknown card'
  }

}
