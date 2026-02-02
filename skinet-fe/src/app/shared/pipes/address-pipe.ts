import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { ShippingAddress } from '../models/order';

@Pipe({
  name: 'address'
})
export class AddressPipe implements PipeTransform {

  transform(value?: ConfirmationToken["shipping"] | ShippingAddress, ...args: unknown[]): string {
    if (value && 'address' in value && value.name) {
      //Declare 7 separates variable with respectively value 
      const { city, country, line1, line2, postal_code, state } = (value as ConfirmationToken["shipping"])?.address!;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city}, ${state ? state + ', ' : ''} ${postal_code}, ${country}`;
    } else if (value && 'postalCode' in value) {
      const { city, country, line1, line2, postalCode, state } = value as ShippingAddress;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city}, ${state ? state + ', ' : ''} ${postalCode}, ${country}`;
    }
    return 'Unknown address';
  }

}
