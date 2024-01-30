import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-author-detail',
  templateUrl: './author-detail.component.html',
  styleUrls: ['./author-detail.component.css']
})
export class AuthorDetailComponent {

  authorId: number=0;
  authorDetail:any;
  booksByAuthorId: any;
  responsiveOptions: any[] | undefined;
  products: any | undefined;

  constructor
  (
    private route : ActivatedRoute,
    private http : HttpClient,
    private error : ErrorService
  )
  {
      this.route.params.subscribe(params=>{
        this.authorId = params['value'];
      });
      this.getAuthorDetail();
      this.getBooksByAuthorId();
      this.responsiveOptions = [
        {
            breakpoint: '1400px',
            numVisible: 3,
            numScroll: 3
        },
        {
            breakpoint: '1220px',
            numVisible: 2,
            numScroll: 2
        },
        {
            breakpoint: '1100px',
            numVisible: 1,
            numScroll: 1
        }
    ];
  }

  titleMaxCharacter(title:string):string{
    if(title.length<25)
    return title;

    return title.substring(0,25) + "...";
  }

  getBooksByAuthorId(){
    this.http.get("https://localhost:7062/api/Authors/GetBooksByAuthorId/"+this.authorId).subscribe({
      next: (res:any) => {
        this.booksByAuthorId = res;
        this.products = this.booksByAuthorId.result2
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }

  getAuthorDetail(){
    this.http.get("https://localhost:7062/api/Authors/GetAuthorDetailById/"+this.authorId).subscribe({
      next: (res:any) => {
        this.authorDetail = res;
      },
      error : (err : HttpErrorResponse) => {
        this.error.errorHandlerBr(err);
      }
    });
  }
}
