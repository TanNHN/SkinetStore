import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { PaymentSummary } from '../models/order';

@Pipe({
  name: 'paymentCard'
})
export class PaymentCardPipe implements PipeTransform {

  transform(value?: ConfirmationToken['payment_method_preview'] | PaymentSummary, ...args: unknown[]): string {
    if (value && 'card' in value) {
      const { display_brand, exp_month, exp_year, last4 } = (value as ConfirmationToken['payment_method_preview']).card!
      return `${display_brand?.toUpperCase()} **** **** **** ${last4}, ${exp_month}/${exp_year}`
    } else if(value && 'brand' in value){
      const { brand, expMonth, expYear, last4 } = value as PaymentSummary;
      return `${brand?.toUpperCase()} **** **** **** ${last4}, ${expMonth}/${expYear}`
    }
    return 'Unknown card'
  }
}
