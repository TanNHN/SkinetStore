import { inject, Injectable } from '@angular/core';
import { forkJoin, of, tap } from 'rxjs';
import { User } from '../../shared/models/user';
import { AccountService } from './account.service';
import { CartService } from './cart.service';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private cartService = inject(CartService);
  private accountService = inject(AccountService);
  private signalrService = inject(SignalrService);

  init() {
    const cartId = localStorage.getItem('cart_id');
    const cart$ = cartId ? this.cartService.getCart(cartId) : of(null);
    const user: User = {
      email: 'anon',
      address: { city: '', country: '', line1 :'', line2: '', postalCode: '', state: ''},
      firstName: 'anon',
      lastName: 'anon'
    }
    return of({
      cart: null,
      user: user
    })
    //folk join => allow to wait multiple observables to complete and emit last value to array 
    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo().pipe(
        tap(user => {
          if (user) this.signalrService.createHubConnection();
        })
      )
    });
  }
}
