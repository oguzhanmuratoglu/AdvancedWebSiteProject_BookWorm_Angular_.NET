import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from 'src/app/services/error.service';
import { SharedService } from 'src/app/services/shared.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { PopupService } from 'src/app/services/popup.service';
import { UserModel } from 'src/app/models/user.model';
import { MessageService } from 'primeng/api';
import { NgForm } from '@angular/forms';
import { SignInModel } from 'src/app/models/sign-in.model';
import { AuthService } from 'src/app/services/auth.service';
import { CartItemModel } from 'src/app/models/cart-item.model';
import { CartModel } from 'src/app/models/cart.model';
import { Router } from '@angular/router';
import { SwalService } from 'src/app/services/swal.service';
import { WishlistService } from 'src/app/services/wishlist.service';
import { WishlistRequestModel } from 'src/app/models/wishlist-request.model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements AfterViewInit {

  categories : any[] =[];
  selectedCategory: string = "All Categories";
  user:UserModel = new UserModel();
  signInModel : SignInModel = new SignInModel();
  wishlistRequestModel: WishlistRequestModel = new WishlistRequestModel();
  loopForCategoriesDiv: any[] = [0,1,2,3,4,5,6,7,8];
  @ViewChild('categoryBtn') categoryBtn!: ElementRef;
  isPasswordFocus:boolean =false;
  isShowPassword: boolean = false;
  isShowCnfrmPassword:boolean = false;
  searchItem:string = "";


  
  constructor(
    private http : HttpClient,
    public shared : SharedService,
    public shop : ShoppingCartService,
    private popup : PopupService,
    private messageService : MessageService,
    private error : ErrorService,
    public auth : AuthService,
    private swal : SwalService,
    private router : Router,
    public wish : WishlistService,

  )
  {
    this.getAllCategories();
    if (this.router.url.includes("search-list")) {
      const encodedSearchTerm = this.router.url.split("search-list/")[1];
      this.searchItem = decodeURIComponent(encodedSearchTerm);
    }
  }

  searchProducts(){
    if (this.searchItem && this.selectedCategory) {
      const encodedSearchTerm = encodeURIComponent(this.searchItem);
      const encodedAnotherParameter = encodeURIComponent(this.selectedCategory);
      window.location.href = "/search-list/" + encodedSearchTerm + "/" + encodedAnotherParameter;
  }
  }

  goToWishList(){
    if(this.auth.checkAuthentication()){
      const currentRoute = this.router.url;
      if(!currentRoute.includes("my-account")){
        this.router.navigateByUrl("/my-account").then(res=> {
          const el : any = document.getElementById("pills-six-example1-tab");
          const el2 : any = document.getElementById("buttonId");
          el.click();
          el2.click();
        });
      }
    }else{
      window.location.href = "/wishlist";
    }
  }

  logout(){
    localStorage.removeItem("auth");
    this.swal.callToastBe("Logout process is successful")
    window.location.href = "/";
  }


  signIn(form : NgForm){

    if (form.valid) {
      this.http.post("https://localhost:7062/api/Users/LoginUser", this.signInModel)
      .subscribe({

        next : (res:any)=>{

          localStorage.setItem("auth",JSON.stringify(res));
          this.auth.checkAuthentication();
          if(this.auth.checkAuthentication()){
            this.setDataFromLocalToDb();
          }
          this.messageService.add({ 
            key: "bl2", 
            severity: 'success', 
            summary: 'Success', 
            detail: "Login Process Successful"
          });
          window.location.href= "/";
        },
        error : (err : HttpErrorResponse) => {
          this.error.errorHandlerBl(err);
        }
      })

    }
    else
    {
      this.swal.callToastBs("Please try again without leaving any free space.", "warning");
    }
  }



  setDataFromLocalToDb(){
    if(this.shop.shoppingCart.length >0){

      const shoppingCart = new CartModel();
      shoppingCart.userId = this.auth.token.userId;
      for(let s of this.shop.shoppingCart)
      {
        const cartItem = new CartItemModel();
        cartItem.bookVariationId = s.book.bookVariations[0].id;
        cartItem.quantity = s.quantity;
        shoppingCart.shoppingCartItems.push(cartItem)
      }

      this.http.post("https://localhost:7062/api/ShoppingCarts/SetDataFromLocalStorage",shoppingCart).subscribe({
        next:(res:any)=>{
          localStorage.removeItem("shoppingCart");
          this.shop.getShoppingCart();
        },
        error:(err:HttpErrorResponse)=>{
          this.error.errorHandlerBl(err);
        }
      })

    }
    if(this.wish.wishCard.length>0){
      const bookVariationIds = [];
      for(let w of this.wish.wishCard){
        bookVariationIds.push(w.book.bookVariations[0].id);
      }
      this.wishlistRequestModel.bookVariationIds = bookVariationIds;
      this.wishlistRequestModel.userId = this.auth.token.userId;
      this.http.post("https://localhost:7062/api/Wishlistes/SetWishListFromLocalStorage", this.wishlistRequestModel).subscribe({
        next:(res:any)=>{
          localStorage.removeItem("wishCard");
          this.wish.getWishCart();
        },
        error:(err:HttpErrorResponse)=>{
          this.error.errorHandlerBl(err);
        }
      })
    }
  }

  signUp(form:NgForm){
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};:'\\|,.<>\/?])(?=.*[0-9]).+$/;
    const str = form.controls["password"].value;
    const confrm = form.controls["confirmPassword"].value;
    const isValid = regex.test(str);

    if (form.valid && isValid && confrm==str) {
      this.http.post("https://localhost:7062/api/Users/CreateUser", this.user).subscribe({
        next : (res:any)=>{
          this.user = new UserModel();
          this.messageService.add({ key: "bl", severity: 'success', 
          summary: 'Success', detail: res.message});
        },
        error : (err : HttpErrorResponse) => {
          this.error.errorHandlerBl(err);
        }
      })
    }else{
      this.messageService.add({ key: "bl", severity: 'error', 
        summary: 'Error', detail: 'Please fill out the form according to the rules.'})
    }
  }


  removeItemFromCart(id:number, event:Event):void{

    this.popup.confirmDeleted(event, 'Are you sure that you want to proceed?', (res)=>{

      if(this.auth.checkAuthentication()){

        this.http.post("https://localhost:7062/api/ShoppingCarts/RemoveCartItemFromDatabase" , {userId:this.auth.token.userId, bookVariationId:id}).subscribe({
          next: (res:any)=>{
            this.shop.getShoppingCart();
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });

      }else{
        this.shop.shoppingCart = this.shop.shoppingCart.filter(item => item.book.bookVariations[0].id !== id);
        localStorage.setItem("shoppingCart", JSON.stringify(this.shop.shoppingCart));
      }

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

  ngAfterViewInit() {
    this.shared.setCategoryDiv(this.categoryBtn);
  }

  getAllCategories(){
    this.http.get("https://localhost:7062/api/Categories/GetAllCategories").subscribe({
      next: (res:any)=>{
        this.categories = res;
      },
      error: (err:HttpErrorResponse)=>{
      }
    })
  }

  checkRegex(el:HTMLInputElement)
  {
    const text = el.value;

    //Büyük Harf kontrolü
    const upperCaseRegex = /[A-Z]/;
    const upperCaseResult = upperCaseRegex.test(text);
    const upperLetterEl = document.getElementById("upperLetter");
    upperLetterEl?.classList.add(upperCaseResult ? 'pw-success': 'pw-error');
    upperLetterEl?.classList.remove(!upperCaseResult ? 'pw-success': 'pw-error');

    //küçük Harf kontrolü
    const lowerCaseRegex = /[a-z]/;
    const lowerCaseResult = lowerCaseRegex.test(text);
    const lowerLetterEl = document.getElementById("lowerLetter");
    lowerLetterEl?.classList.add(lowerCaseResult ? 'pw-success': 'pw-error');
    lowerLetterEl?.classList.remove(!lowerCaseResult ? 'pw-success': 'pw-error');


    //Özel karakter kontrolü 
    const specialCaseRegex = /[!@#$%^&*()_+\-=\[\]{};:'\\|,.<>\/?]+/;
    const specialCaseResult = specialCaseRegex.test(text);
    const specialLetterEl = document.getElementById("specialLetter");
    specialLetterEl?.classList.add(specialCaseResult ? 'pw-success': 'pw-error');
    specialLetterEl?.classList.remove(!specialCaseResult ? 'pw-success': 'pw-error');


    //Sayı  kontrolü  
    const numeraticCaseRegex = /[0-9]/;
    const numeraticCaseResult = numeraticCaseRegex.test(text);
    const numeraticLetterEl = document.getElementById("numeraticLetter");
    numeraticLetterEl?.classList.add(numeraticCaseResult ? 'pw-success': 'pw-error');
    numeraticLetterEl?.classList.remove(!numeraticCaseResult ? 'pw-success': 'pw-error');


    //6karakter kısmı kontrolü 
    const minSixCharecterEl = document.getElementById("minSixCharecter");
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

}
