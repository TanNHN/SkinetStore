import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { Pagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    HeaderComponent,
  ],

  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  products = signal<Product[]>([]);
  constructor(
    private http: HttpClient,
    private ref: ChangeDetectorRef
  ) { }
  // or 
  // private http = inject(HttpClient)
  //After constructor has been initialized, next event is OnInit()

  ngOnInit(): void {
    //get return observable
    //But think of observables as streams of data that we can listen to or more accurately subscribe to.
    //And in order to observe something, then we have to subscribe to it.
    //Curly brackets in subscribe() because we do this inside an observer object.
    //next: data => console.log(data) call back function, a property of observer object is a call back function?
    // if I'm subscribing to something, then possibly I need to unsubscribe from it to prevent it to staty subscribe whole time
    //but not in http request, bc when http finish it call complete() and no longer subcribe to observable
    this.http.get<Pagination<Product>>(this.baseURL + 'products').subscribe({
      next: data => {
        this.products.set(data.data);
        this.ref.detectChanges();
      },
      error: error => console.log(error),
      complete: () => console.log('complete')
    });
  }
  baseURL = 'https://localhost:5001/api/';
  protected readonly title = signal('skinet-fe');
}
