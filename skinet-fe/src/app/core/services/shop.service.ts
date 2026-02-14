import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParam } from '../../shared/models/shopParam';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  private http = inject(HttpClient);
  baseURL = environment.apiUrl;
  types: string[] = [];
  brands: string[] = [];

  getProducts(shopParam: ShopParam) {
    let params = new HttpParams();

    if(shopParam.brands && shopParam.brands.length > 0){
      params = params.append('brands', shopParam.brands.join(','));
    }
    
    if(shopParam.types && shopParam.types.length > 0){
      params = params.append('types', shopParam.types.join(','));
    }

    if(shopParam.sort && shopParam.sort.length > 0){
      params = params.append('sort', shopParam.sort);
    }

    if(shopParam.search){
      params = params.append('search', shopParam.search);
    }
    
    params = params.append('pageIndex', shopParam.pageNumber);
    params = params.append('pageSize', shopParam.pageSize);

    return this.http.get<Pagination<Product>>(this.baseURL + 'products', {params});
  }

  getProduct(id: number){
    return this.http.get<Product>(this.baseURL + `products/${id}`);
  }

  getTypes() {
    if(this.types.length > 0) return;
    return this.http.get<string[]>(this.baseURL + 'products/types').subscribe({
      next: response => this.types = response 
    });
  }

  getBrands() {
    if(this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseURL + 'products/brands').subscribe({
      next: response => this.brands = response 
    });
  }
}
