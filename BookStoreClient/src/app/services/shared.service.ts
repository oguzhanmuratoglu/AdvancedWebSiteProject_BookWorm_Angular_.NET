import { JsonPipe } from '@angular/common';
import { ElementRef, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  private categoryBtn: any;
  private allBtn: any;
  isShow:boolean=false;
  
  constructor() { }



  setCategoryDiv(categoryBtn: any) {
    this.categoryBtn = categoryBtn;
  }

  getCategoryDiv(): any {
    return this.categoryBtn;
  }

  setAllBtn(allBtn: any) {
    this.allBtn = allBtn;
  }

  getAllBtn(): any {
    return this.allBtn;
  }
}
