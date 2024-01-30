import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SwalService {

  constructor() { }

  callToastBe(title: string, icon: SweetAlertIcon = "success"){
    const Toast = Swal.mixin({
      toast: true,
      position: 'bottom-end',
      timer: 3000,
      timerProgressBar: true,
      showConfirmButton: false
    });
    Toast.fire(title, '', icon);
  }

  callToastBs(title: string, icon: SweetAlertIcon = "success"){
    const Toast = Swal.mixin({
      toast: true,
      position: 'bottom-start',
      timer: 3000,
      timerProgressBar: true,
      showConfirmButton: false
    });
    Toast.fire(title, '', icon);
  }

  callSwalQuestion(title: string, cancelBtnName: string, confirmBtnName: string,callBack: ()=> void){
    Swal.fire({
      title: title,
      icon: "question",
      showCancelButton: true,
      cancelButtonText: cancelBtnName,
      showConfirmButton: true,
      confirmButtonText: confirmBtnName
    }).then(res=> {
      if(res.isConfirmed){
        callBack();
      }
    });
  }
}

type SweetAlertIcon = 'success' | 'error' | 'warning' | 'info' | 'question'
