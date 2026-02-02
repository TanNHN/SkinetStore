import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { checkoutGuard } from './core/guards/checkout-guard';
import { checkoutSuccessGuard } from './core/guards/checkout-success-guard';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { CartComponent } from './features/cart/cart.component';
import { CheckoutSuccessComponent } from './features/checkout/checkout-success/checkout-success.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { ErrorComponent } from './features/error/error.component';
import { HomeComponent } from './features/home/home.component';
import { OrderDetailComponent } from './features/orders/order-detail/order-detail.component';
import { OrderComponent } from './features/orders/order/order.component';
import { ProductDetailComponent } from './features/shop/product-detail/product-detail.component';
import { ShopComponent } from './features/shop/shop.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'shop', component: ShopComponent },
    { path: 'shop/:id', component: ProductDetailComponent },
    { path: 'cart', component: CartComponent },
    { path: 'checkout', component: CheckoutComponent, canActivate: [authGuard, checkoutGuard] },
    { path: 'checkout/success', component: CheckoutSuccessComponent, canActivate: [authGuard, checkoutSuccessGuard] },
    { path: 'orders', component: OrderComponent, canActivate: [authGuard] },
    { path: 'orders/:id', component: OrderDetailComponent, canActivate: [authGuard] },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'error', component: ErrorComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },


    { path: '**', redirectTo: '', pathMatch: 'full' },
];
