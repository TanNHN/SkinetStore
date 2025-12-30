import { Component, inject, OnInit, signal } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatAnchor } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";
import { MatFormField, MatLabel } from "@angular/material/select";
import { MatDivider } from "@angular/material/divider";
import { MatInput } from "@angular/material/input";
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-product-detail',
  imports: [
    CurrencyPipe,
    MatAnchor,
    MatIcon,
    MatFormField,
    MatLabel,
    MatDivider,
    MatInput,
    FormsModule
  ],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss',
})
export class ProductDetailComponent implements OnInit {

  private shopService = inject(ShopService);
  private acivatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);

  product = signal<Product | null>(null);
  quantityInCart = 0;
  quantity = 1;

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.acivatedRoute.snapshot.paramMap.get('id');

    if (!id) {
      return;
    }
    this.shopService.getProduct(+id).subscribe({
      next: result => {
        this.product.set(result)
        this.updateQuantityInCart();
      },
      error: err => console.log(err)
    });
  }

  updateQuantityInCart() {
    // 0 || 'default'      // "default" ❌ (0 is valid!)
    this.quantityInCart = this.cartService.cart()?.items.find(p => p.productId === this.product()?.id)?.quantity || 0;
    this.quantity = this.quantityInCart || 1;
  }

  getButtonText() {
    return this.quantityInCart > 0 ? "Update cart" : "Add to cart"
  }

  updateCart() {
    const product = this.product();
    if (!product) return;
    if (this.quantity > this.quantityInCart) {
      const itemToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemToAdd;
      this.cartService.addItemToCart(product, itemToAdd);
    } else {
        const itemToRemove = this.quantityInCart - this.quantity;
        this.quantityInCart -= itemToRemove;
        this.cartService.removeItemFromCart(product.id, itemToRemove);
    }
  }
}
