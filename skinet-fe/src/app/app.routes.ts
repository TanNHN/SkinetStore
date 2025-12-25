import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailComponent } from './features/shop/product-detail/product-detail.component';
import { ErrorComponent } from './features/error/error.component';
import { ServerErrorComponent } from './shared/component/server-error/server-error.component';
import { NotFoundComponent } from './shared/component/not-found/not-found.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'shop', component: ShopComponent },
    { path: 'shop/:id', component: ProductDetailComponent },
    { path: 'error', component: ErrorComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },

    { path: '**', redirectTo: '', pathMatch: 'full' },
];
