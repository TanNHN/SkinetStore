import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog'
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParam } from '../../shared/models/shopParam';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule,
    MatIconButton
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {


  products = signal<Pagination<Product>>({
    count: 0,
    data: [],
    pageIndex: 0,
    pageSize: 10
  });

  pageSizeOption = [5, 10, 15, 20]

  dialogService = inject(MatDialog);
  sortOptions = [
    { name: 'Alphabet', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' },
  ]
  shopParam = new ShopParam();

  constructor(
    private ref: ChangeDetectorRef,
    private shopService: ShopService
  ) { }
  // or 
  // private http = inject(HttpClient)
  //After constructor has been initialized, next event is OnInit()

  ngOnInit(): void {
    this.initShop();
  }

  initShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  onSearchChange(){
    this.shopParam.pageNumber = 1;
    this.getProducts();
  }

  getProducts() {
    //get return observable
    //But think of observables as streams of data that we can listen to or more accurately subscribe to.
    //And in order to observe something, then we have to subscribe to it.
    //Curly brackets in subscribe() because we do this inside an observer object.
    //next: data => console.log(data) call back function, a property of observer object is a call back function?
    // if I'm subscribing to something, then possibly I need to unsubscribe from it to prevent it to staty subscribe whole time
    //but not in http request, bc when http finish it call complete() and no longer subcribe to observable
    this.shopService.getProducts(this.shopParam).subscribe({
      next: result => this.products.set(result),
      error: err => console.log(err)
    });
  }

  openFilterDialog() {
    const dialogRef = this.dialogService.open(FilterDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: this.shopParam.brands,
        selectedTypes: this.shopParam.types
      }
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.shopParam.brands = result.selectedBrands;
          this.shopParam.types = result.selectedTypes;
          this.shopParam.pageNumber = 1;
          this.getProducts();
        }
      }
    })
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOpt = event.options[0];
    if (selectedOpt) {
      this.shopParam.sort = selectedOpt.value;
      this.shopParam.pageNumber = 1;
      this.getProducts();
    }
  }

  handlePageEvent(event: PageEvent) {
    this.shopParam.pageNumber = event.pageIndex + 1;
    this.shopParam.pageSize = event.pageSize;
    this.getProducts();
  }
}
