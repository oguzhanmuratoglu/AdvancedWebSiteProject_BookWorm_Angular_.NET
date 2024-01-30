import { Component } from '@angular/core';
import { ShippingTypes } from 'src/app/constants/shipping-types';
import { PopupService } from 'src/app/services/popup.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from 'src/app/services/error.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { CartItemModel } from 'src/app/models/cart-item.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  providers: []
})
export class CartComponent {

  selectedShippingMethod: any = null;
  isReadOnly:boolean = false;
  couponCode:string = "";
  couponDetail:any;
  hasCouponData:boolean=false;
  shippingTypes: any[] = ShippingTypes;




  constructor
  (
    public shop : ShoppingCartService,
    public popup : PopupService,
    private confirmationService: ConfirmationService,
    private http : HttpClient,
    private error : ErrorService,
    private messageService: MessageService,
    public auth : AuthService,
    private router : Router
  )
  {
    this.shop.getShoppingCart(()=>{
      this.checkFreeShipping();
    });
  }

  checkUserLogin(event:MouseEvent){
    if (this.auth.checkAuthentication()) {
      this.shop.checkoutModel.couponDetail = this.couponDetail;
      this.shop.checkoutModel.shippingModule.shippingPrice = this.selectedShippingMethod.price;
      this.shop.checkoutModel.shippingModule.shippingType = this.selectedShippingMethod.name;
      this.shop.checkoutModel.totalPrice = this.calculateTotalLastPrice();
      localStorage.removeItem("checkout");
      localStorage.setItem("checkout", JSON.stringify(this.shop.checkoutModel))
      localStorage.removeItem("checkoutCard");
      localStorage.setItem("checkoutCard", JSON.stringify(this.shop.shoppingCart))
      this.router.navigateByUrl("/checkout");
    }else{
      this.popup.loginWarning(event);
    }
  }


  calculateTotalLastPrice():string{
    let total = this.shop.calcualteTotalCartItem().totalPriceAmount;
    let shippingPrice = this.selectedShippingMethod.key;
    let couponPrice =0;
    if(this.hasCouponData){
      if(this.couponDetail.discountPercentage == null){
        couponPrice = this.couponDetail.discountAmount
      }else{
        let discount = 100-this.couponDetail.discountPercentage;
        total = total*discount/100;
      }
    }
    let result = total + shippingPrice - couponPrice; 
    if(result<0) return "0₺"

    result = Number(result.toFixed(2))

    return `${result}₺`;
  }

  checkCouponCode(){
    this.http.get("https://localhost:7062/api/ShoppingCarts/CheckCouponsCode/"+this.couponCode).subscribe({
      next: (res : any)=>{
        this.couponDetail = res;
        this.hasCouponData = res.isActivated;
        this.changeBorderColor(res);
        this.messageService.add({ key: "br", severity: 'success', 
        summary: 'Success', detail: 'Coupon Found and Discount Successfully Provided'})
      },
      error: (err:HttpErrorResponse)=>{
        this.error.errorHandlerBr(err)
        this.hasCouponData = err.error.isActivated;
        this.changeBorderColor(err.error);
      }
    })
  }
  changeBorderColor(response: any){
    const el : any = document.getElementById("couponTag");
    if(response.isActivated){
      el.classList.remove("border-danger");
      el.classList.add("border-success");
    }else{
      el.classList.remove("border-success");
      el.classList.add("border-danger");
    }
  }

  showWarningPopupForCart(event: any, index:number ){
    if(+event.target.value > this.shop.shoppingCart[index].book.bookVariations[0].quantity){

      this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: 'You cannot enter more than the maximum stock amount.',
        icon: 'pi pi-exclamation-triangle',
        acceptVisible: false,
        rejectVisible: false,
    });
    event.target.value = this.shop.shoppingCart[index].book.bookVariations[0].quantity;
    this.changeItemQuantity(event, index);
    }
    else if(+event.target.value <= 0){
      this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: "You cannot enter a number less than 1",
        icon: 'pi pi-exclamation-triangle',
        acceptVisible: false,
        rejectVisible: false,
    });
    event.target.value = 1;
    this.changeItemQuantity(event, index);
    }
    else{
      this.changeItemQuantity(event, index);
    }
  }

  changeItemQuantity(event:any, index:number){
    if (this.auth.checkAuthentication()) {
      const cartItem = new CartItemModel();
      cartItem.bookVariationId = +this.shop.shoppingCart[index].book.bookVariations[0].id;
      cartItem.quantity = +event.target.value;
      this.http.post("https://localhost:7062/api/ShoppingCarts/ChangeShoppingCartItemQuantity",{
        userId:this.auth.token.userId,
        bookVariationId: cartItem.bookVariationId,
        quantity : cartItem.quantity
      }).subscribe({
        next: (res:any)=>{
          this.shop.getShoppingCart(()=>{
          this.checkFreeShipping()
          })
        },
        error : (err: HttpErrorResponse)=>{
          this.error.errorHandlerBl(err);
        }
      });
    }else{
      this.shop.shoppingCart[index].quantity = +event.target.value;
      this.shop.setShoppingCartToLocalStorage();
      this.checkFreeShipping();
    }
  }

  checkFreeShipping() {
      const totalPrice = this.shop.calcualteTotalCartItem().totalPriceAmount;
  
      if (totalPrice >= 100) {
        this.selectedShippingMethod = this.shippingTypes.find(type => type.key === 0);
        this.disableOtherShippingTypes();
      } else {
        this.enableAllShippingTypes();
      }
  }

  disableOtherShippingTypes() {
    for (const type of this.shippingTypes) {
      if (type.key === 0) {
        type.disabled = false;
        this.selectedShippingMethod = type;
      } else {
        type.disabled = true;
      }
    }
  }
  enableAllShippingTypes() {
    for (const type of this.shippingTypes) {
      if (type.key === 0) {
        type.disabled = true;
      } else {
        type.disabled = false;
        this.selectedShippingMethod = type;
      }
    }
  }

  getBookFormat(formatId: number): string {
    switch (formatId) {
      case 0:
        return "HARDCOVER";
      case 1:
        return "AUDIOBOOK";
      case 2:
        return "KINDLE";
      default:
        return "UNKNOWN FORMAT";
    }
  }

  removeItemToCart(bookVariationId:number , index:number):void{
    if (this.auth.checkAuthentication()) {

      this.http.post("https://localhost:7062/api/ShoppingCarts/RemoveCartItemFromDatabase" , {userId:this.auth.token.userId, bookVariationId:bookVariationId}).subscribe({
          next: (res:any)=>{
            this.shop.getShoppingCart(()=>{
              this.checkFreeShipping();
            })
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });
    
    
    } else {
    this.shop.shoppingCart.splice(index,1);
    this.shop.setShoppingCartToLocalStorage();
    
    this.checkFreeShipping();
    }
  }

  calculateTotalPrice(index:number):string{
    let price = this.shop.shoppingCart[index].book.bookVariations[0].prices[0].discountedPriceAmount 
    ?? this.shop.shoppingCart[index].book.bookVariations[0].prices[0].priceAmount

    let totalPrice = price*this.shop.shoppingCart[index].quantity;
    totalPrice = Number(totalPrice.toFixed(2));
    return `${totalPrice} ₺`
  }
}
