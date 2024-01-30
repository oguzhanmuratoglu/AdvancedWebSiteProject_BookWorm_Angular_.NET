import { OrderItemModel } from "./order-item-model";
import { ShippingModule } from "./shipping-module";

export class OrderModel
{
    userId:number=0;
    shippingAddress:any;
    billingAddress:any;
    shippingModule: ShippingModule = new ShippingModule();
    orderItems : OrderItemModel[]=[];
    totalOrderPriceAmount:number=0;
    priceCurrency:string="";
    totalOrderQuantity:number=0;
    couponDetail:any;
    orderNotes:string="";
}