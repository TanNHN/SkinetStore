import { Component, inject } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';
import { MatFormField, MatLabel } from "@angular/material/select";
import { CurrencyPipe, Location } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-order-summary',
  imports: [MatFormField, MatLabel, CurrencyPipe, MatButton, MatInput, RouterLink],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss',
})
export class OrderSummaryComponent {
  cartService = inject(CartService);
  location = inject(Location);
}
