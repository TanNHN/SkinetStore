import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { Order } from '../../../shared/models/order';
import { AddressPipe } from '../../../shared/pipes/address-pipe';
import { PaymentCardPipe } from '../../../shared/pipes/payment-pipe';

@Component({
  selector: 'app-order-detail',
  imports: [MatCardModule, MatButton, DatePipe, CurrencyPipe, AddressPipe, PaymentCardPipe, RouterLink],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.scss'
})
export class OrderDetailComponent implements OnInit {
  private orderService = inject(OrderService);
  order = signal<Order | null>(null);
  private activatedRoute = inject(ActivatedRoute);
  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id && Number.isNaN(id)) return;
    this.orderService.getOrderDetail(Number(id)).subscribe({
      next: res => this.order.set(res)
    });
  }
}
