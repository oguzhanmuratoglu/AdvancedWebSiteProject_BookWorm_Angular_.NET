import { Component } from '@angular/core';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { SwalService } from 'src/app/services/swal.service';
import { WishlistService } from 'src/app/services/wishlist.service';

@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.css']
})
export class WishlistComponent {


  constructor
  (
    public wish : WishlistService,
    public shop : ShoppingCartService,
    private swal : SwalService,
  
  )
  {
  }

  removeItemFromWishlist(bookId:number){
    this.wish.wishCard = this.wish.wishCard.filter(w=>w.book.bookId != bookId);
    this.wish.setwishCardToLocalStorage();
    this.swal.callToastBe("This item successfully removed")
  }
}
