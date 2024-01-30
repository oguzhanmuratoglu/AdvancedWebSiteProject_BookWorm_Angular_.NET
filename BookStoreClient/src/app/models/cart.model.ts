import { CartItemModel } from "./cart-item.model";

export class CartModel
{
    userId:number=0;
    shoppingCartItems:CartItemModel[]=[];
}