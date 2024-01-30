import { Injectable } from '@angular/core';
import { ShoppingCartItemModel } from '../models/shopping-cart-item.model';
import { NavbarCartModel } from '../models/navbar-cart.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from './auth.service';
import { ErrorService } from './error.service';
import { SwalService } from './swal.service';
import { CheckoutModel } from '../models/checkout.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  shoppingCart:ShoppingCartItemModel[]=[];
  checkoutModel : CheckoutModel=new CheckoutModel();
  navbarCart: NavbarCartModel = new NavbarCartModel();
  constructor
  (
    private http : HttpClient,
    private auth : AuthService,
    private error : ErrorService,
    private swal : SwalService
  )
  {
    this.getShoppingCart();
  }

  addItemToCart(variationId:number, selectedBook:any){
    if (this.auth.checkAuthentication()) {

      this.http.post("https://localhost:7062/api/ShoppingCarts/AddItemToCart" , {
        userId:this.auth.token.userId,
        bookVariationId: variationId, 
        quantity:1})
        .subscribe({
          next: (res:any)=>{
            this.getShoppingCart();
            this.swal.callToastBe("The product has been successfully added to your cart");
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });
      
    } else {
    const book = {...selectedBook}
    book.bookVariations = 
    book.bookVariations.filter((bv: any) => bv.id == variationId);

    let checkBookIsAlreadyExists = 
    this.shoppingCart.find(p => p.book.bookVariations[0].id == variationId)


    if(checkBookIsAlreadyExists !== undefined){
      checkBookIsAlreadyExists.quantity += 1;
    }
    else{
      const newItem: ShoppingCartItemModel = {
        book: book,
        quantity: 1
      };
      this.shoppingCart.push(newItem);
    }
    this.setShoppingCartToLocalStorage();
    this.swal.callToastBe("The product has been successfully added to your cart");

    }
    
  }

 getShoppingCart(callBack?:()=>void){
    const cart = localStorage.getItem("shoppingCart");
    if (cart && cart!== null) {
        this.shoppingCart = JSON.parse(cart);
        if (callBack) {
          callBack();
          }
    }else{
      this.shoppingCart = [];
    }

    if(this.auth.checkAuthentication()){
    this.http.get(`https://localhost:7062/api/ShoppingCarts/GetDataFromDatabase/${this.auth.token.userId}`).subscribe({

        next: (res:any)=>{
          this.shoppingCart = res[0].shoppingCartItems
          if (callBack) {
          callBack();
          }

        },
        error: (err:HttpErrorResponse)=>{
          this.error.errorHandlerBl(err)
        }
      })
    }
  }

  setShoppingCartToLocalStorage(){
    localStorage.setItem("shoppingCart", JSON.stringify(this.shoppingCart));
  }

  calcualteTotalCartItem(){
    this.navbarCart.totalItemCount =0;
    this.navbarCart.totalPriceAmount = 0;
    this.navbarCart.priceCurrency = this.shoppingCart.map(item => item.book.bookVariations[0].prices[0].priceCurrency)[0];
    this.shoppingCart.forEach((item)=>{

      this.navbarCart.totalItemCount += item.quantity;

      this.navbarCart.totalPriceAmount += 
      (item.book.bookVariations[0].prices[0].discountedPriceAmount 
      ?? item.book.bookVariations[0].prices[0].priceAmount) * item.quantity;
    });
    
    this.navbarCart.totalPriceAmount = Number(this.navbarCart.totalPriceAmount.toFixed(2));
    return this.navbarCart
  }
}
