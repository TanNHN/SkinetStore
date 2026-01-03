export interface User {
    firstName: string;
    lastName: string;
    email: string;
    address: Address;
}

export interface Address {
    streetAddress: string;
    ward: string;
    district: string;
    province: string;
    postalCode: string;
    country: string;
}