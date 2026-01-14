import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'address'
})
export class AddressPipe implements PipeTransform {

  transform(value?: ConfirmationToken["shipping"], ...args: unknown[]): string {
    if (value?.address && value.name) {
      //Declare 7 separates variable with respectively value 
      const { city, country, line1, line2, postal_code, state } = value.address;
      return `${value.name}, ${line1}${line2 ? ', ' + line2 : ''}, ${city}, ${state ? state + ', ' : ''} ${postal_code}, ${country}`;
    }
    return 'Unknown address';
  }

}
