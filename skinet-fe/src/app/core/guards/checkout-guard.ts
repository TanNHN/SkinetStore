import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { inject } from '@angular/core';
import { SnackbarService } from '../services/snackbar.service';

export const checkoutGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const snackService = inject(SnackbarService);
  const router = inject(Router);

  if (!cartService.cart() || cartService.cart()?.items.length === 0) {
    snackService.error('Your cart is empty');
    router.navigateByUrl('/cart');
    return false;
  }
  return true;
};
