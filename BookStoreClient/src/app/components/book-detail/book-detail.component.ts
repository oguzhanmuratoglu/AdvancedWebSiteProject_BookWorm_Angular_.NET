import { DatePipe } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {  Component  } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReviewPercentageModel } from 'src/app/models/review-percentage.model';
import { ErrorService } from 'src/app/services/error.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BookDetailRequestModel } from 'src/app/models/book-detail-request.model';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { ShoppingCartItemModel } from 'src/app/models/shopping-cart-item.model';
import { ToastService } from 'src/app/services/toast.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css'],
  providers: []
})
export class BookDetailComponent{

  bookId:number =0;
  safeVideoUrl: SafeResourceUrl="";
  response: any;
  selectedVariationId: number=0;
  images: any[] | undefined;
  request : BookDetailRequestModel = new BookDetailRequestModel();
  reviewPercantages : ReviewPercentageModel = new ReviewPercentageModel();
  relatedBooks: any;
  prices : any[]=[];
  minPrice: number=0;
  maxPrice: number=0;
  quantity: number=1;

  responsiveOptions: any[] = [
    {
        breakpoint: '1024px',
        numVisible: 5
    },
    {
        breakpoint: '768px',
        numVisible: 3
    },
    {
        breakpoint: '560px',
        numVisible: 1
    }
];
  
  
  constructor
  (
    private route : ActivatedRoute,
    private http : HttpClient,
    private error : ErrorService,
    private sanitizer: DomSanitizer,
    private datePipe: DatePipe,
    public shopping : ShoppingCartService,
    private toast : ToastService,
    private auth : AuthService
  )
  {
    this.route.params.subscribe(params=>{
      this.bookId = params['value'];
    });

    this.getBook();
    this.getRelatedBooks();
    this.increaseVisitedCount();
  }

  increaseVisitedCount(){
    this.http.get(`https://localhost:7062/api/Books/IncreaseVisitedCount/${this.bookId}`).subscribe({
      next: (res:any)=>{
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBl(err);
      }
    });
  }

  addItemToCart(){
    if (this.auth.checkAuthentication()) {

      this.http.post("https://localhost:7062/api/ShoppingCarts/AddItemToCart" , {
        userId:this.auth.token.userId,
        bookVariationId:this.selectedVariationId, 
        quantity:this.quantity})
        .subscribe({
          next: (res:any)=>{
            this.shopping.getShoppingCart();
          },
          error : (err: HttpErrorResponse)=>{
            this.error.errorHandlerBl(err);
          }
        });
      
    } else {
    const book = {...this.response.data[0]}
    book.bookVariations = 
    book.bookVariations.filter((bv: any) => bv.id == this.selectedVariationId);

    let checkBookIsAlreadyExists = 
    this.shopping.shoppingCart.find(p => p.book.bookVariations[0].id == this.selectedVariationId)


    if(checkBookIsAlreadyExists !== undefined){
      checkBookIsAlreadyExists.quantity += this.quantity;
    }
    else{
      const newItem: ShoppingCartItemModel = {
        book: book,
        quantity: this.quantity
      };
      this.shopping.shoppingCart.push(newItem);
    }
    this.shopping.setShoppingCartToLocalStorage();
    this.toast.showSuccess("The product has been successfully added to your cart");

    }
    
  }
  

  getMax28Character(value:string):string{
    if(value.length<=28) return value;

    return value.substring(0,28) + "...";
  }

  getRelatedBooks(){
    this.http.get("https://localhost:7062/api/Books/GetRelatedBooks").subscribe(res=>{
      this.relatedBooks = res;
    });
  }

  onModalHidden(index: number) {
    // index parametresini kullanarak, ilgili videoyu durdurma işlemini gerçekleştirin
    const iframe = document.getElementById(`videoIframe${index}`) as HTMLIFrameElement;
    if (iframe) {
        iframe.contentWindow?.postMessage('{"event":"command","func":"stopVideo","args":""}', '*');
    }
}

  getSafeVideoUrl(videoUrl:string):SafeResourceUrl{
    this.safeVideoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(videoUrl);
    return this.safeVideoUrl;
  }
  max70Character(title:string):string{
    if(title.length<70)
    return title;

    return title.substring(0,70) + '...';
  }

  max20Character(title:string):string{

    if(title.length<20)
    return title;

    return title.substring(0,20) + '...';
  }

  getUserName(id: number): string {
    const user = this.response.userReviews.find((u:any) => u.userId === id);
  
    if (user) {
      return user.userName;
    } else {
      return "";
    }
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return this.datePipe.transform(date, 'MMMM dd, yyyy') || '';

  }
  feedData() {
    if(this.response.reviewCount+5>this.request.reviewSize)
    {
      this.request.reviewSize += 3;
      this.getBook();
    }
  }

  getBook() {
    this.request.bookId = this.bookId;
    this.http.post("https://localhost:7062/api/Books/GetSingleBook/",this.request).subscribe({
      next : (res : any) =>{
        this.response = res;
        this.images = res.data[0].bookImgUrls;
        this.selectedVariationId = this.response.data[0].bookVariations[0].id
        this.convertToString(res.starPercentages)
        this.minMaxPrice(res.data[0].bookVariations)
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err)
      }
    })
  }

  minMaxPrice(datas : any){
    if(this.prices.length) return;
    
    for(let data of datas){
      for(let price of data.prices){
        this.prices.push(price.discountedPriceAmount ?? price.priceAmount);
      }
    }
    this.prices = this.prices.filter(price => price !== null && price !== undefined);
    this.minPrice = Math.min(...this.prices);
    this.maxPrice = Math.max(...this.prices);
  }

  convertToString(starPercentages:any)
  {
      this.reviewPercantages.fiveStarPercentage = `${starPercentages.fiveStarPercentage}%`;
      this.reviewPercantages.fourStarPercentage = `${starPercentages.fourStarPercentage}%`;
      this.reviewPercantages.threeStarPercentage = `${starPercentages.threeStarPercentage}%`;
      this.reviewPercantages.twoStarPercentage = `${starPercentages.twoStarPercentage}%`;
      this.reviewPercantages.oneStarPercentage = `${starPercentages.oneStarPercentage}%`;
  }
}
