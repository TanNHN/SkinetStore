import { CurrencyPipe, Location } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatFormField, MatLabel } from "@angular/material/select";
import { RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { CartService } from '../../../core/services/cart.service';
import { SnackbarService } from '../../../core/services/snackbar.service';

@Component({
  selector: 'app-order-summary',
  imports: [MatFormField, MatLabel, CurrencyPipe, MatButton, MatInput, RouterLink, ReactiveFormsModule, MatIcon],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss',
})
export class OrderSummaryComponent implements OnInit {

  private fb = inject(FormBuilder);
  cartService = inject(CartService);
  location = inject(Location);
  snackService = inject(SnackbarService);
  isDisableVoucherInput = signal(false);

  voucherForm = this.fb.group({
    VoucherCode: ['']
  });

  ngOnInit(): void {
    this.setVoucherStatus((this.cartService.cart()?.coupon && this.cartService.cart()?.coupon?.valid) ?? false);
  }

  getVoucher() {
    this.cartService.applyCoupon(this.voucherForm.value.VoucherCode!).subscribe({
      next: async result => {
        this.cartService.cart.update(cart => {
          if (cart) {
            cart.coupon = result;
          }
          return cart;
        });
        await firstValueFrom(this.cartService.setCart(this.cartService.cart()!));
        this.setVoucherStatus(true);
      }, error: _ => {
        this.snackService.error("Invalid voucher code");
      }
    });
  }

  removeVoucher() {
    this.cartService.removeCoupon();
    this.isDisableVoucherInput.set(false);
    this.voucherForm.get('VoucherCode')?.enable()
  }

  private setVoucherStatus(status: boolean) {
    if (status) {
      this.isDisableVoucherInput.set(true);
      this.voucherForm.get('VoucherCode')?.disable()
    } else {
      this.isDisableVoucherInput.set(false);
      this.voucherForm.get('VoucherCode')?.enable()
    }
  }
}
