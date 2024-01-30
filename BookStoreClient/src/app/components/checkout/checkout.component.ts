import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { CheckoutBillingAddressModel } from 'src/app/models/checkout-billing-address-model';
import { CheckoutShippingAddressModel } from 'src/app/models/checkout-shipping-address-model';
import { OrderItemModel } from 'src/app/models/order-item-model';
import { OrderModel } from 'src/app/models/order-model';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorService } from 'src/app/services/error.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { SwalService } from 'src/app/services/swal.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {

  isSameAddress:boolean=true;
  checkoutModel:any;
  totalOrderPrice:number=0;
  userAddress:any;
  orderNotes:string="";
  shippingAddress: CheckoutShippingAddressModel=new CheckoutShippingAddressModel();
  billingAddress : CheckoutBillingAddressModel=new CheckoutBillingAddressModel();

  constructor
  (
    public shop :ShoppingCartService,
    private http : HttpClient,
    private auth : AuthService,
    private error : ErrorService,
    private swal : SwalService,
    private spinner: NgxSpinnerService,
    private router : Router

  )
  {
    if (localStorage.getItem("checkout")) {
      this.checkoutModel = JSON.parse(localStorage.getItem("checkout") as any);
    }
    if (this.shop.shoppingCart.length==0) { 
      if (localStorage.getItem("checkoutCard")) {
        this.shop.shoppingCart = JSON.parse(localStorage.getItem("checkoutCard") as any);
      }
    }
    this.getUserAddresses();
  }


  createOrder(form:NgForm){

    if (form.valid) {
      if (this.isSameAddress) {
        this.billingAddress = {...this.shippingAddress};
        this.billingAddress.addressType = "Billing";
        this.billingAddress.id = 0;
      }
      const orderItems : OrderItemModel[]=[];
      for(let s of this.shop.shoppingCart){
        let price = s.book.bookVariations[0].prices[0].discountedPriceAmount ?? s.book.bookVariations[0].prices[0].priceAmount;
        const orderItem = new OrderItemModel();
        orderItem.bookVariationId = s.book.bookVariations[0].id;
        orderItem.priceCurrency = s.book.bookVariations[0].prices[0].priceCurrency;
        orderItem.quantity = s.quantity;
        orderItem.totalPriceAmount = Number((price * s.quantity).toFixed(2));
        orderItems.push(orderItem);
      }
      const order = new OrderModel();
      order.userId = +this.auth.token.userId;
      order.billingAddress = this.billingAddress;
      order.billingAddress.userId = order.userId
      order.shippingAddress= this.shippingAddress;
      order.shippingAddress.userId = order.userId
      order.shippingModule = this.checkoutModel.shippingModule;
      if (this.checkoutModel.couponDetail) {
        order.couponDetail = this.checkoutModel.couponDetail;
      }else{
        order.couponDetail = null;
      }
      order.orderItems = orderItems;
      order.priceCurrency = orderItems[0].priceCurrency;
      order.totalOrderPriceAmount = this.totalOrderPrice;
      order.totalOrderQuantity = this.shop.shoppingCart.reduce((total, item) => total + item.quantity, 0);
      order.orderNotes = this.orderNotes;
      this.http.post("https://localhost:7062/api/Orders/CreateOrder", order).subscribe({
        next: (res:any)=>{
          this.spinner.show();
          setTimeout(() => {
            this.spinner.hide();
          }, 5000);
          window.location.href=`/order-received/${res.number}`;
        },
        error : (err: HttpErrorResponse)=>{
          this.spinner.hide();
            this.error.errorHandlerBr(err);
        }
      });

    }else{
      this.swal.callToastBe("Please fill in all the requested information completely.","warning")
    }
  }

  maxCharacter(value : string):string{
    if (value.length<60) {
      return value;
    } else {
      return value.substring(0,60) + "...";
    }
  }

  setAddress(shippingType:number){
    if (shippingType===1) {
      this.shippingAddress = this.userAddress.shippingAddress;
      this.swal.callToastBe("Shipping data has been transferred successfully")
    } else {
      this.billingAddress = this.userAddress.billingAddress;
      this.swal.callToastBe("Billing data has been transferred successfully")
    }
  }
  getUserAddresses(){
    this.http.get(`https://localhost:7062/api/Users/GetUserAddresses/${this.auth.token.userId}`).subscribe({
      next: (res:any)=>{
        this.userAddress = res;
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBl(err);
      }
    })
  }

  calculateTotalLastPrice():string{
    let total = this.shop.calcualteTotalCartItem().totalPriceAmount;
    let shippingPrice = this.removeLastCharacter(this.checkoutModel.shippingModule.shippingPrice)
    let couponPrice =0;
    if(this.checkoutModel.couponDetail){
      if(this.checkoutModel.couponDetail.discountPercentage == null){
        couponPrice = this.checkoutModel.couponDetail.discountAmount
      }else{
        let discount = 100-this.checkoutModel.couponDetail.discountPercentage;
        total = total*discount/100;
      }
    }
    let result = total + shippingPrice - couponPrice; 

    if(result<0) return "0₺"

    result = Number(result.toFixed(2))
    this.totalOrderPrice = result;
    return `${result}₺`;
  }

  removeLastCharacter(value: string): number {
    value = value.slice(0, -1);
    return +value;
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


  changeSameAddress(){
    this.isSameAddress = !this.isSameAddress;
  }
}
