import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { SwalService } from './swal.service';
import { WishlistModel } from '../models/wishlist.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {


  wishCard:WishlistModel[]=[];
  constructor
  (
    private auth : AuthService,
    private swal :SwalService,
    private http : HttpClient,
    private error : ErrorService
  )
  {
    this.getWishCart();
  }

  getWishCart(){
    const cart = localStorage.getItem("wishCard");
    if (cart && cart!== null) {
        this.wishCard = JSON.parse(cart);
    }else{
      this.wishCard = [];
    }
    if(this.auth.checkAuthentication()){
      this.http.get(`https://localhost:7062/api/Wishlistes/GetWishList/${this.auth.token.userId}`).subscribe({ 
        next: (res:any)=>{
          this.wishCard = res;
        },
        error: (err:HttpErrorResponse)=>{
          this.error.errorHandlerBl(err)
        }

      });
    }
  }

  addItemTowishCard(variationId:number, selectedBook:any)
  {
    if(this.auth.checkAuthentication()){

    }else{
      const book = {...selectedBook}
      let checkBookIsAlreadyExists = 
      this.wishCard.find(p => p.book.bookVariations[0].id == variationId)
  
  
      if(checkBookIsAlreadyExists !== undefined){
        this.swal.callToastBe("This item has already been on your wishlist.", "warning")
      }
      else{
        const newItem: WishlistModel = {
          book: book,
          bookVariationId: variationId,
        };
        this.wishCard.push(newItem);
        this.setwishCardToLocalStorage();
        this.swal.callToastBe("The product has been successfully added to your wishlist");
      }
    }
  }

  setwishCardToLocalStorage(){
    localStorage.setItem("wishCard", JSON.stringify(this.wishCard));
  }
}
