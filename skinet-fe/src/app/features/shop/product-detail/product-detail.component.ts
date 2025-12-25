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


@Component({
  selector: 'app-product-detail',
  imports: [
    CurrencyPipe,
    MatAnchor,
    MatIcon,
    MatFormField,
    MatLabel,
    MatDivider,
    MatInput
],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss',
})
export class ProductDetailComponent implements OnInit {

  private shopService = inject(ShopService);
  private acivatedRoute = inject(ActivatedRoute);
  product = signal<Product | null>(null);

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.acivatedRoute.snapshot.paramMap.get('id');

    if (!id) {
      return;
    }
    this.shopService.getProduct(+id).subscribe({
      next: result => this.product.set(result),
      error: err => console.log(err)
    });
  }
}
