import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom, map, tap } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Cart, CartItem } from '../../shared/models/cart';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient)

  cart = signal<Cart | null>(null);
  deliveryMethod = signal<DeliveryMethod | null>(null);

  totals = computed(() => {
    if (!this.cart()) return null;
    const subTotal = this.cart()?.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const shipping = this.deliveryMethod()?.price ?? 0;
    const discount = 0;
    return {
      subTotal,
      shipping,
      discount,
      total: subTotal! - discount + shipping
    }
  });

  itemCount = computed(() => {
    // reduce((prev, current))
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });

  getCart(id: string) {
    return this.http.get<Cart>(`${this.baseUrl}cart?id=${id}`).pipe(
      map(cart => {
        this.cart.set(cart);
        return cart;
      })
    )
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(`${this.baseUrl}cart`, cart).pipe(
      tap(cart => {
        this.cart.set(cart);
      })
    )
  }

  //CartItem has similar prop as Product but for Product ID in CartItem is ProductID and Product is ID
  async addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart();
    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    await firstValueFrom(this.setCart(cart));
  }

  async removeItemFromCart(productID: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;
    const index = cart.items.findIndex(x => x.productId === productID);
    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity -= quantity;
      } else {
        console.log(cart);

        cart.items.splice(index, 1);
        console.log(cart);
      }
      if (cart.items.length === 0) {
        this.deleteCart();
      } else {
        await firstValueFrom(this.setCart(cart));
      }
    }
  }

  deleteCart() {
    this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id)
      .subscribe({
        next: () => {
          localStorage.removeItem('cart_id');
          this.cart.set(null)
        }
      });
  }

  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    const index = items.findIndex(p => p.productId === item.productId);
    if (index == -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      price: item.price,
      type: item.type,
      quantity: 0
    }
  }

  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart
  }
}
