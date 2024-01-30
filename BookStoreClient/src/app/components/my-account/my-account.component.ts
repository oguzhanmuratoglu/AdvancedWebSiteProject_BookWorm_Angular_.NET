import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { PasswordModel } from 'src/app/models/password.model';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorService } from 'src/app/services/error.service';
import { PopupService } from 'src/app/services/popup.service';
import { SharedService } from 'src/app/services/shared.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { SwalService } from 'src/app/services/swal.service';
import { WishlistService } from 'src/app/services/wishlist.service';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.css']
})
export class MyAccountComponent {

  selectedUserId:number=0;
  userInformation : any;
  passwordModel: PasswordModel = new PasswordModel();
  isPasswordFocus:boolean =false;
  isShowPassword: boolean = false;
  isShowCnfrmPassword:boolean = false;
  userAddress:any;
  orders: any;

  constructor
  (
    public auth : AuthService,
    private swal : SwalService,
    public wish : WishlistService,
    private http : HttpClient,
    private error : ErrorService,
    public shop :ShoppingCartService,
    public shared : SharedService,
    private popup : PopupService,

  )
  {
    this.userInformation = {...this.auth.token};
    this.getUserAddresses();
    this.getOrders();
  }

  getOrderStatus(statusNumber : any): string{
    var result = "";
    switch (statusNumber) {
      case 0:
        result = "Awaiting Approval" 
        break;

      case 1:
        result = "Preparing"
        break;

      case 2:
        result = "In Transit"
        break;
      
      case 3:
        result = "Delivered"
        break;

      case 4:
        result = "Rejected"
        break;

      case 5:
        result = "Returned"
        break;

      default:
        break;
    }

    return result;
  }

  getOrders(){
    this.http.get(`https://localhost:7062/api/Orders/GetOrderStatus/${this.auth.token.userId}`).subscribe({
      next: (res:any)=>{
        this.orders = res;
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBl(err);
      }
    })
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

  showOrHidePassword(password:HTMLInputElement){
    if(this.isShowPassword){
      this.isShowPassword = false;
      password.type = "password"
    }else{
      this.isShowPassword = true;
      password.type = "text"
    }
  }
  showOrHideConfirmPassword(password:HTMLInputElement){
    if(this.isShowCnfrmPassword){
      this.isShowCnfrmPassword = false;
      password.type = "password"
    }else{
      this.isShowCnfrmPassword = true;
      password.type = "text"
    }
  }
  checkRegex(el:HTMLInputElement)
  {
    const text = el.value;

    //Büyük Harf kontrolü
    const upperCaseRegex = /[A-Z]/;
    const upperCaseResult = upperCaseRegex.test(text);
    const upperLetterEl = document.getElementById("upperLetter2");
    upperLetterEl?.classList.add(upperCaseResult ? 'pw-success': 'pw-error');
    upperLetterEl?.classList.remove(!upperCaseResult ? 'pw-success': 'pw-error');

    //küçük Harf kontrolü
    const lowerCaseRegex = /[a-z]/;
    const lowerCaseResult = lowerCaseRegex.test(text);
    const lowerLetterEl = document.getElementById("lowerLetter2");
    lowerLetterEl?.classList.add(lowerCaseResult ? 'pw-success': 'pw-error');
    lowerLetterEl?.classList.remove(!lowerCaseResult ? 'pw-success': 'pw-error');


    //Özel karakter kontrolü 
    const specialCaseRegex = /[!@#$%^&*()_+\-=\[\]{};:'\\|,.<>\/?]+/;
    const specialCaseResult = specialCaseRegex.test(text);
    const specialLetterEl = document.getElementById("specialLetter2");
    specialLetterEl?.classList.add(specialCaseResult ? 'pw-success': 'pw-error');
    specialLetterEl?.classList.remove(!specialCaseResult ? 'pw-success': 'pw-error');


    //Sayı  kontrolü  
    const numeraticCaseRegex = /[0-9]/;
    const numeraticCaseResult = numeraticCaseRegex.test(text);
    const numeraticLetterEl = document.getElementById("numeraticLetter2");
    numeraticLetterEl?.classList.add(numeraticCaseResult ? 'pw-success': 'pw-error');
    numeraticLetterEl?.classList.remove(!numeraticCaseResult ? 'pw-success': 'pw-error');


    //6karakter kısmı kontrolü 
    const minSixCharecterEl = document.getElementById("minSixCharecter2");
    minSixCharecterEl?.classList.add(text.length <6 ? 'pw-error': 'pw-success');
    minSixCharecterEl?.classList.remove(text.length >= 6 ? 'pw-error': 'pw-success');

    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};:'\\|,.<>\/?])(?=.*[0-9]).+$/;
    const isValid=regex.test(text);

    if (isValid && text.length >= 6) {
      this.isPasswordFocus = false;
      el.classList.add("is-valid");
      el.classList.remove("is-invalid");
    } else {
      this.isPasswordFocus = true;
      el.classList.remove("is-valid");
      el.classList.add("is-invalid");
    }
  }

  updateUserPassword(event:MouseEvent){

    this.popup.confirmDeleted(event, 'It is necessary to log out for this operation. Are you sure you want to change your password?', 
    (res)=>{
      const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};:'\\|,.<>\/?])(?=.*[0-9]).+$/;
      const isValid = regex.test(this.passwordModel.newPassword);

      if (isValid && this.passwordModel.confirmNewPassword === this.passwordModel.newPassword && this.passwordModel.currentPassword !="") {
        this.http.post("https://localhost:7062/api/Users/ChangeUserPassword", 
        {
          userId:this.auth.token.userId,
          currentPassword: this.passwordModel.currentPassword,
          newPassword : this.passwordModel.newPassword,
        }
        ).subscribe({
          next: (res:any)=>{
            this.logoutdefault();
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });
      }else{
        this.swal.callToastBe("Please try again by fulfilling the required conditions.", "error")
      }
  })
  }

  updateUserInformation(event:MouseEvent){

    this.popup.confirmDeleted(event, 'It is necessary to log out for this operation. Are you sure you want to change your user information?', 
    (res)=>{
        this.http.post("https://localhost:7062/api/Users/UpdateUserInformation" , this.userInformation).subscribe({
          next: (res:any)=>{
            this.logoutdefault();
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });
  })
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

  removeWishItemFromDatabase(id:number){
    this.http.post("https://localhost:7062/api/Wishlistes/RemoveWishListItemByUserId",{userId:this.auth.token.userId, bookVariationId:id}).subscribe({
      next: (res:any)=>{
        this.wish.getWishCart();
        this.swal.callToastBe("This item successfully removed")
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBl(err);
      }
    });
  }

  clickItem(id:string){
      const el:any = document.getElementById(id);
      el?.click();
  }

  logout(event:MouseEvent){
    this.popup.confirmDeleted(event, 'Are you sure you want to log out?', 
    (res)=>{
      this.logoutdefault();
  });
    
  }

  logoutdefault(){
    localStorage.removeItem("auth");
    this.swal.callToastBe("Logout process is successful")
    window.location.href = "/";
  }
}
