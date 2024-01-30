import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ErrorService } from 'src/app/services/error.service';
import { SwalService } from 'src/app/services/swal.service';

@Component({
  selector: 'app-authors-list',
  templateUrl: './authors-list.component.html',
  styleUrls: ['./authors-list.component.css']
})
export class AuthorsListComponent {

  authors:any[]=[];
  authors2:any[]=[];
  selectedLetter: string | null="";
  firstLetterOfAuthors: any;

  constructor
  (
    private http: HttpClient,
    private swal : SwalService,
    private err : ErrorService

  )
  {
    this.callApi();
  }

  getAuthorsFromFirstLetter(letter:string | null){
    this.selectedLetter = letter;
    this.authors = this.authors2;
    if(letter !== null){
      this.authors = this.authors.filter(c => c.fullName.charAt(0).toUpperCase() === letter.toUpperCase());
    }
  }

  callApi(){
    this.http.get("https://localhost:7062/api/Authors/GetAuthorsFirstLetter").subscribe({
      next: (res:any) => {
        this.firstLetterOfAuthors = res;
        this.getAllAuthors();
      },
      error : (err : HttpErrorResponse) => {
        this.err.errorHandlerBr(err);
      }
    });
  }

  getAllAuthors()
  {
    this.http.get("https://localhost:7062/api/Authors/GetAllAuthors").subscribe({
      next: (res:any) => {
        this.authors = res;
        this.authors2 = res;
      },
      error : (err : HttpErrorResponse) => {
        this.err.errorHandlerBr(err);
      }
    });
  }


}
