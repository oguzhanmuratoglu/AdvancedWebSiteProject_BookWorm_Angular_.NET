import { AfterViewInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements AfterViewInit{
  constructor
  (
    private shared : SharedService,
    private router : Router

  )
  {
  }
  ngAfterViewInit(){
    if(!(this.router.url === "/")){
        this.shared.getCategoryDiv()?.nativeElement.click();
    }
  }
}
