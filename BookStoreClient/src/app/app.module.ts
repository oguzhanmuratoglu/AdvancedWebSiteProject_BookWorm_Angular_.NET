import { NgModule , CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './components/layout/layout.component';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { ToastModule } from 'primeng/toast';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { CategoryComponent } from './components/category/category.component';
import { BooksListByCategoryComponent } from './components/books-list-by-category/books-list-by-category.component';
import { FormsModule } from '@angular/forms';
import { BookDetailComponent } from './components/book-detail/book-detail.component';
import { DatePipe } from '@angular/common';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { GalleriaModule } from 'primeng/galleria';
import { AuthorsListComponent } from './components/authors-list/authors-list.component';
import { AuthorDetailComponent } from './components/author-detail/author-detail.component';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { BookListByAuthorComponent } from './components/book-list-by-author/book-list-by-author.component';
import { CartComponent } from './components/cart/cart.component';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ConfirmationService, MessageService } from 'primeng/api';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ValidateDirective } from './directives/validate.directive';
import { ConfirmDirective } from './directives/confirm.directive';
import { MyAccountComponent } from './components/my-account/my-account.component';
import { WishlistComponent } from './components/wishlist/wishlist.component';
import { Confirm2Directive } from './directives/confirm2.directive';
import { AddressComponent } from './components/my-account/address/address.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { AccordionModule } from 'primeng/accordion';
import { NgxSpinnerModule } from 'ngx-spinner';
import { OrderReceivedComponent } from './components/order-received/order-received.component';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { SearchListComponent } from './components/search-list/search-list.component';
        


export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}


@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    CategoryComponent,
    BooksListByCategoryComponent,
    BookDetailComponent,
    AuthorsListComponent,
    AuthorDetailComponent,
    BookListByAuthorComponent,
    CartComponent,
    ValidateDirective,
    ConfirmDirective,
    MyAccountComponent,
    WishlistComponent,
    Confirm2Directive,
    AddressComponent,
    CheckoutComponent,
    OrderReceivedComponent,
    SearchListComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SweetAlert2Module,
    AppRoutingModule,
    BrowserAnimationsModule,
    ButtonModule,
    AccordionModule,
    GalleriaModule,
    RadioButtonModule,
    NgxSpinnerModule.forRoot({ type: 'pacman' }),
    ToastModule,
    ConfirmPopupModule,
    CarouselModule,
    InfiniteScrollModule,
    FormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  schemas:[CUSTOM_ELEMENTS_SCHEMA],
  providers: [DatePipe,ConfirmationService, MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
