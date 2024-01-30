import { Injectable } from '@angular/core';
import { Months } from '../constants/months';

@Injectable({
  providedIn: 'root'
})
export class DateService {
  months:any = Months
  constructor() { }

  getCurrentMonth(selectedLanguage:string):string{
    const currentMonthIndex = new Date().getMonth();
    if(selectedLanguage==="en"){
      return this.months.en[currentMonthIndex];
    }
    return this.months.tr[currentMonthIndex];
  }
}
