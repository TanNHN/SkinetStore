import { nanoid } from 'nanoid';

export interface CartType {
    id: string;
    items: CartItem[];
    deliveryMethodId?: number;
    paymentIntentId?: string;
    clientSecret?: string;
    coupon?: Coupon
}

export interface CartItem {
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export class Cart implements CartType {
    id = nanoid();
    items: CartItem[] = [];
    deliveryMethodId?: number;
    paymentIntentId?: string;
    clientSecret?: string;
    coupon?: Coupon;
}

export interface Coupon {
    id: string;
    amountOff: number;
    percentOff: number;
    duration: string;
    durationInMonths: string;
    maxRedemptions: string;
    name: string;
    valid: boolean;
}