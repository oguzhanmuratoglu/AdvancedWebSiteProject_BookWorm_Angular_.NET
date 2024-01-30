import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AddressModel } from 'src/app/models/address.model';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorService } from 'src/app/services/error.service';
import { SwalService } from 'src/app/services/swal.service';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css']
})
export class AddressComponent {

  addressType:string="";
  userAddress:AddressModel =new AddressModel();
  constructor
  (
    private route:ActivatedRoute,
    private swal : SwalService,
    private http : HttpClient,
    private error : ErrorService,
    private auth : AuthService,
    private router : Router
  )
  {
    this.route.params.subscribe(params=>{
      this.addressType = params['value'];
    });
    this.getUserAddresses();
  }

  

  changeUserAddress(form:NgForm){
    if(form.valid){
      this.http.post("https://localhost:7062/api/Users/ChangeUserAddress",this.userAddress).subscribe({
      next: (res:any)=>{
        this.router.navigateByUrl("/my-account").then(res=>{
          this.swal.callToastBe("Your address information has been changed successfully.")
        });
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBr(err);
      }

      });
    }else{
      this.swal.callToastBe("Please enter the information completely", "error")
    }
  }

  getUserAddresses(){
    this.http.post(`https://localhost:7062/api/Users/GetUserAddressesByType`,{
      userId: this.auth.token.userId,
      addressType : this.addressType
    }).subscribe({
      next: (res:any)=>{
        this.userAddress = res;
      },
      error : (err: HttpErrorResponse)=>{
        this.error.errorHandlerBr(err);
      }
    })
  }
}
