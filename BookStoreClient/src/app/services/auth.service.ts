import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token.model';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  token: TokenModel = new TokenModel();
  tokenString: string = "";
  constructor(private router: Router) { }

  checkAuthentication() {
    const isAuth = localStorage.getItem("auth");
    if (!isAuth) {
      return false;
    }
    const responseJson = JSON.parse(isAuth);
    this.tokenString = responseJson?.accessToken;
    if (!this.tokenString) {
      return false;
    }
  
    const decode:any = jwtDecode(this.tokenString);
    this.token.email = decode?.Email;
    this.token.firstName = decode?.FirstName;
    this.token.lastName = decode?.LastName;
    this.token.userName = decode?.UserName;
    this.token.userId = decode?.UserId;
    this.token.exp = decode?.exp;
  
    const now = new Date().getTime() / 1000;
    if (this.token.exp < now) {
      return false;
    }
  
    return true;
  }
  
}
