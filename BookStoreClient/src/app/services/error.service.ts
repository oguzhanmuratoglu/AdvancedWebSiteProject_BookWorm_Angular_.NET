import { Injectable } from '@angular/core';
import { SwalService } from './swal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  
  constructor(
    private swal: SwalService,
    private translate: TranslateService,
    private messageService : MessageService,
    private router: Router
    ) { }

errorHandlerBr(err: HttpErrorResponse){
  console.log(err);

  switch (err.status) {
    case 0:
      this.swal.callToastBe("The server is currently unavailable. Please try again later","warning")
      break;
    
    case 400:
      this.swal.callToastBe(err.error.message,"warning")
      break;
  
    case 404:
      this.swal.callToastBe(err.error.message,"warning")
      break;
  
    case 500:
      this.swal.callToastBe(err.error.message,"warning")
      break;

    default:
      this.swal.callToastBe("An Undetermined Error Occurred, Please Try Again.", "error")
      break;
  }
}

errorHandlerBl(err: HttpErrorResponse){
  
  switch (err.status) {
    case 0:
      this.swal.callToastBs("The server is currently unavailable. Please try again later","warning")
      break;
    
    case 400:
      this.swal.callToastBs(err.error.message,"warning")
      break;
  
    case 404:
      this.swal.callToastBs(err.error.message,"warning")
      break;
  
    case 500:
      this.swal.callToastBs(err.error.message,"warning")
      break;

    default:
      this.swal.callToastBs("An Undetermined Error Occurred, Please Try Again.", "error")
      break;
  }
}
}
