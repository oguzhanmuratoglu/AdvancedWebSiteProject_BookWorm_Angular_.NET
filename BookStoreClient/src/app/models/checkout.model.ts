import { ShippingModule } from "./shipping-module";

export class CheckoutModel
{
    couponDetail:any;
    shippingModule:ShippingModule= new ShippingModule();
    totalPrice:string="";
}