import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { OrderService } from '../services/order.service';

export const checkoutSuccessGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const orderService = inject(OrderService);
  if(!orderService.orderComplete()){
    router.navigateByUrl('/shop');
    return false;
  }
  return true;
};
