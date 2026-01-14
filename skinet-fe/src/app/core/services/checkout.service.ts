import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = environment.apiUrl;
  private httpService = inject(HttpClient);
  deliveryMethods = signal<DeliveryMethod[]>([]);

  getDeliveryMethods() : Observable<DeliveryMethod[]> {
    if (this.deliveryMethods.length > 0) return of(this.deliveryMethods());
    return this.httpService.get<DeliveryMethod[]>(this.baseUrl + 'payment/delivery-methods').pipe(
      map(deliveryMethods => {
        this.deliveryMethods.set(deliveryMethods.sort((a,b) => a.price - b.price));
        return deliveryMethods;
      })
    )
  }
}
