export interface Order {
  id: number
  orderDate: string
  buyerEmail: string
  shippingAddress: ShippingAddress
  deliveryMethod: string
  paymentSummary: PaymentSummary
  orderItems: OrderItem[]
  subTotal: number
  shippingPrice: number
  total: number
  status: string
  paymentIntentId: string
}

export interface ShippingAddress {
  name: string
  line1: string
  line2: string
  city: string
  state: string
  postalCode: string
  country: string
}

export interface PaymentSummary {
  last4: number
  brand: string
  expMonth: number
  expYear: number
}

export interface OrderItem {
  productId: number
  productName: string
  pictureUrl: string
  quantity: number
  price: number
}

export interface OrderToCreate{
    cartId: string;
    deliveryMethodId: number;
    shippingAddress: ShippingAddress;
    paymentSummary: PaymentSummary;
}