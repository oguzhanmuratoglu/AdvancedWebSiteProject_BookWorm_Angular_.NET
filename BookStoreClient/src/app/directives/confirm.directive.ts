import { Directive, ElementRef, HostListener } from '@angular/core';
import { UserModel } from '../models/user.model';
import { SharedService } from '../services/shared.service';

@Directive({
  selector: '[confirm]'
})
export class ConfirmDirective {

  constructor
  (

    private el:ElementRef<HTMLInputElement>,
    private shared: SharedService

  ) { }

  checkSameValue(){
    const paswordEl:any = document.getElementById("password");

    if(paswordEl.value == this.el.nativeElement.value){
      this.el.nativeElement.classList.add("is-valid");
      this.el.nativeElement.classList.remove("is-invalid");
      this.shared.isShow=false;
    }else{
      this.el.nativeElement.classList.add("is-invalid");
      this.el.nativeElement.classList.remove("is-valid");
      this.shared.isShow=true;
    }
  }

  @HostListener("keyup") keyup(){
    this.checkSameValue();
  }

}
