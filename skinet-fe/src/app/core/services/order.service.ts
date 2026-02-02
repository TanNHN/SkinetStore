import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Order, OrderToCreate } from '../../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  orderComplete = signal(false);
  createOrder(orderToCreate: OrderToCreate) {
    return this.http.post(this.baseUrl + 'orders', orderToCreate);
  }

  getOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }

  getOrderDetail(id: number) {
    return this.http.get<Order>(this.baseUrl + 'orders/' + id);
  }
}
