import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { ErrorService } from 'src/app/services/error.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent{


  selectedLetter:string | null = "";
  categories:any[] = [];
  categories2:any[] = [];
  filterCategories : any[] = [];
  firstLetterOfCategories : any[] = [];

  constructor
  (
    private http : HttpClient,
    private error : ErrorService,
    private shared : SharedService
  )
  {
    this.callApi();
  }

  

  
  getCategoriesFromFirstLetter(letter:string | null){
    this.selectedLetter = letter;
    this.categories = this.categories2;
    if(letter !== null){
      this.categories = this.categories.filter(c => c.name.charAt(0).toUpperCase() === letter.toUpperCase());
    }
  }

  callApi(){
    this.http.get("https://localhost:7062/api/Categories/GetCategoriesFirstLetter").subscribe({
      next: (res:any) => {
        this.firstLetterOfCategories = res;
        this.getCategories();
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }

  getCategories(){
    this.http.get("https://localhost:7062/api/Categories/GetAllCategories").subscribe({
      next: (res:any) => {
        this.categories = res;
        this.categories2 = res;
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }

  

}
