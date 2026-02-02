import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { inject } from '@angular/core';
import { map, of } from 'rxjs';

// return observable of boolean
export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  if (accountService.currentUser()) {
    return of(true);
  } else {
    // No need to subscribe here since guard will deal with sup/unsupcribe
    return accountService.getAuthState().pipe(
      map(response => {
        if (response.isAuthenticated) {
          return true;
        } else {
          router.navigate(['/account/login'], { queryParams: { returnUrl: state.url } });
          return false;
        }
      })
    );
  }
};
