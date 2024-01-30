import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { identifierName } from '@angular/compiler';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-order-received',
  templateUrl: './order-received.component.html',
  styleUrls: ['./order-received.component.css']
})
export class OrderReceivedComponent {

  orderUniqeNumber : string = "";
  orderDetail : any


  constructor
  (
    private route : ActivatedRoute,
    private http : HttpClient,
    private error : ErrorService,


  )
  {
    this.route.params.subscribe(params=>{
      this.orderUniqeNumber = params['value'];
    });

    this.getOrderDetail();
    
  }
  getOrderDetail(){
    this.http.get("https://localhost:7062/api/Orders/GetOrderDetail/"+ this.orderUniqeNumber).subscribe({
      next: (res:any)=>{
        this.orderDetail = res;
        console.log(this.orderDetail)
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBr(err);
      }
    });
  }


  getBookFormat(format : any):string{
    var result = "";
    switch (format) {
      case 0:
        result = "Hardcover" 
        break;

      case 1:
        result = "Audiobook"
        break;

      case 2:
        result = "Kindle"
        break;

      default:
        break;
    }

    return `(${result})`;
  }

  getPaymentMethod(paymentTypeNumber:any): string{
    var result = "";
    switch (paymentTypeNumber) {
      case 0:
        result = "Cash On Delivery"
        break;

      case 1:
        result = "Credit Card"
        break;

      default:
        break;
    }

    return result;
  }

  getBookTitleMax20Chrctr(title : string) : string {
    
    if (title.length >50) {
      var result = title.substring(0,50);
      return `${result}...`;
    }

    return title;
  }
  getShippingType(){
    switch (this.orderDetail.shippingModule.shippingType) {
      case 0:
        return "Free Shipping +0"
      case 1:
        return "Local Shipping +30"
      case 2:
        return "Express Shipping +60"
      default:
        return "";
    }
  }

  totalProductCost(){
    var result = 0;
    for(let item of this.orderDetail.orderItems){
      result += item.totalPriceAmount;
    }
    return result.toFixed(2);
  }
}
