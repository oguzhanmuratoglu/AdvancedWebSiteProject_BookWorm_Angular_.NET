import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookListByCategoryResponseModel } from 'src/app/models/book-list-by-category-response.model';
import { FilterRequestModel } from 'src/app/models/filter-request.model';
import { DateService } from 'src/app/services/date.service';
import { ErrorService } from 'src/app/services/error.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { WishlistService } from 'src/app/services/wishlist.service';

@Component({
  selector: 'app-books-list-by-category',
  templateUrl: './books-list-by-category.component.html',
  styleUrls: ['./books-list-by-category.component.css']
})
export class BooksListByCategoryComponent {

  categoryId:number=0;
  searchValue:string = "";
  selectedLanguage:string ="";
  pageSize:string="10";
  sortFilter:string="default"
  pageNumbers:number[]=[];
  response: BookListByCategoryResponseModel = new BookListByCategoryResponseModel();
  randomBooks: any[] = [];
  categories :any[]=[];
  featuredDatas: any[]=[];
  categoryName : string ="";
  distinctLanguages:any[]=[];
  selectedAuthorNames:any[]=[];
  starArray:number[]=[];
  distinctAuthors: { authorName: string, authorId: number }[] = [];
  toggleStates: { [authorId: number]: boolean } = {};
  selectedAuthors: { [key: number]: boolean } = {};
  selectedLanguages: { [key: string]: boolean } = {};
  selectedReviews: { [key: number]: boolean } = {};
  filterRequestModel:FilterRequestModel = new FilterRequestModel();

  constructor
  (
    private route : ActivatedRoute,
    private http : HttpClient,
    private error : ErrorService,
    public date : DateService,
    public shop : ShoppingCartService,
    public wish : WishlistService,

  )
  {
      this.route.params.subscribe(params=>{
        this.categoryId = params['value'];
      });
      this.getBooksByFilter();
      this.getAllCategories();
      this.getFeaturedDatas();
  }
  
  getFeaturedDatas() {
  this.http.get("https://localhost:7062/api/Books/GetFeaturedBooksAndAuthors").subscribe({
    next: (res: any) => {
      this.featuredDatas = res;
    },
    error: (err: HttpErrorResponse) => {
      this.error.errorHandlerBr(err);
    }
  });
}

  switchSortFilter(event:any){
    this.filterRequestModel.sortFilter = event.target.value.toString();
    this.getBooksByFilter();
  }

  switchPageSize(event:any){
    this.filterRequestModel.pageSize = +event.target.value;
    this.getBooksByFilter();
  }

  previousPage(): void {
      this.filterRequestModel.pageNumber--;
      this.getBooksByFilter();
  }

  nextPage(): void {
      this.filterRequestModel.pageNumber++;
      this.getBooksByFilter();
  }

  setSelectedPageNumber(pageNumber:number): void{
    this.filterRequestModel.pageNumber = pageNumber;
    this.getBooksByFilter();
  }


  getBooksByFilter(){
      const selectedReviews = Object.entries(this.selectedReviews).filter(([key, value]) => value).map(([key, value]) => Number(key));
      this.starArray = [];
      for(let s of selectedReviews){

        this.starArray = this.starArray.concat(this.setArray(s));
      }
      const selectedAuthorIds = Object.entries(this.selectedAuthors).filter(([key, value]) => value).map(([key, value]) => Number(key));
      const selectedLanguages = Object.entries(this.selectedLanguages).filter(([key, value]) => value).map(([key, value]) => (key));

      this.filterRequestModel.authorIds = selectedAuthorIds
      this.filterRequestModel.languages = selectedLanguages
      this.filterRequestModel.categoryId = this.categoryId
      this.filterRequestModel.reviewRatings = this.starArray;


    this.http.post<FilterRequestModel>("https://localhost:7062/api/Books/GetBooksByFilter", this.filterRequestModel)
    .subscribe({
      next: (res:any) => {
        this.response = res;
        this.categoryName = res.categoryName;
        this.setPageNumber();
        this.findDistinctLanguages();
        this.findDistinctAuthors();
        this.getRandomBooks();
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }

  setArray(param: number): number[] {
    const resultArray: number[] = [];
  
    switch (param) {
      case 5:
        resultArray.push(param);
        break;
      case 4:
          for (let i = 4; i <= 4.9; i += 0.1) {
            resultArray.push(Number(i.toFixed(1)));
          }
          break;
      case 3:
          for (let i = 3; i <= 3.9; i += 0.1) {
            resultArray.push(Number(i.toFixed(1)));
          }
          break;
      case 2:
          for (let i = 2; i <= 2.9; i += 0.1) {
            resultArray.push(Number(i.toFixed(1)));
          }
          break;
      case 1:
          for (let i = 1; i <= 1.9; i += 0.1) {
            resultArray.push(Number(i.toFixed(1)));
          }
          break;
    }
  
    return resultArray;
  }

  toggleMethod(authorId: number): void {
    // Toggle the state for the given authorId
    this.toggleStates[authorId] = !this.toggleStates[authorId];
  }

  isToggled(authorId: number): boolean {
    // Check the toggle state for the given authorId
    return this.toggleStates[authorId];
  }

  





  // getBooksByAuthor(id:number){
  //   this.isAuthorSelected = true;
  //   this.books=this.substituteBooks;
  //   if(this.selectedAuthorId === id){
  //     this.isAuthorSelected = false;
  //     this.selectedAuthorId=0;
  //   }else{
  //     this.selectedAuthorId = id;
  //   }

  //   if(this.selectedAuthorId===0){
  //     this.books=this.substituteBooks;
  //   }else{
  //     this.books = this.books.filter(b=>b.authorId === this.selectedAuthorId)
  //   }

  // }

  authorSearch(){

    this.searchValue;
    if(this.searchValue === ""){
      this.findDistinctAuthors();
    }
    else{
      this.distinctAuthors = this.distinctAuthors.filter(b=>b.authorName.toLowerCase().includes(this.searchValue.toLowerCase()));
    }
  }

  getAllCategories() {
    this.http.get("https://localhost:7062/api/Categories/GetAllCategories").subscribe({
      next: (res:any) => {
        this.categories = res;
        const category:any = this.categories.filter(p=>p.id.toString() === this.categoryId);
        this.categoryName = category[0].name;
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }

  getRandomBooks() {
    if (this.response.data.length <= 3) {
      this.randomBooks = this.response.data;
    } else {
      const randomIndices = this.getRandomIndices(3, this.response.data.length);
      this.randomBooks = randomIndices.map(index => this.response.data[index]);
    }
  }

  getRandomIndices(count: number, max: number): number[] {
    const indices:any = [];
    while (indices.length < count) {
      const randomIndex = Math.floor(Math.random() * max);
      if (!indices.includes(randomIndex)) {
        indices.push(randomIndex);
      }
    }
    return indices;
  }

  findDistinctLanguages(): void {
    this.response.data.forEach(book => {
      book.languages.forEach((language: any) => {
        const isLanguageExist = this.distinctLanguages.some(
          lang =>
            lang.languageTr === language.languageTr &&
            lang.languageEn === language.languageEn
        );

        if (!isLanguageExist) {
          this.distinctLanguages.push(language);
        }
      });
    });
  }

  findDistinctAuthors(): void {

    this.response.data.forEach(book => {

        const isAuthorNameExist = this.distinctAuthors.some(
          auth => auth.authorName === book.authorName 
        );

        if (!isAuthorNameExist) {
          this.distinctAuthors.push({authorName:book.authorName , authorId: book.authorId});
        }
      });
  }

  setPageNumber() {
    this.pageNumbers = [];
    for (let i = 0; i < this.response.totalPageCount; i++) {
      this.pageNumbers.push(i + 1);
    }
  }
}


