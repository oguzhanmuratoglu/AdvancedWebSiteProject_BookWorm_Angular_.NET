import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[validate]'
})
export class ValidateDirective {

  constructor
  (
    private el:ElementRef<HTMLInputElement>
  ) { }
  checkValidation(){


    const isValid = this.el.nativeElement.validity.valid;
    if(isValid){
      this.el.nativeElement.classList.add("is-valid");
      this.el.nativeElement.classList.remove("is-invalid");
    }else{
      this.el.nativeElement.classList.add("is-invalid");
      this.el.nativeElement.classList.remove("is-valid");

      const spanEl:any = document.querySelector(`#${this.el.nativeElement.id} + .invalid-feedback`);
      spanEl.innerHTML = this.el.nativeElement.validationMessage;
    }
    this.capitalizeFirstLetter();
  }

  @HostListener("keyup") keyup(){
    this.checkValidation();
  }

  capitalizeFirstLetter() {
    if(this.el.nativeElement.id==="userName" || this.el.nativeElement.id==="userLastName"){
      const inputValue = this.el.nativeElement.value;
        if (inputValue.length > 0) {
          this.el.nativeElement.value = inputValue.charAt(0).toUpperCase() + inputValue.slice(1);
        }
    }
  }
}
