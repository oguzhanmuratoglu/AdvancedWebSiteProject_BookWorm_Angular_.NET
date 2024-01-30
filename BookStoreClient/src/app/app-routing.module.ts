import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { HomeComponent } from './components/home/home.component';
import { CategoryComponent } from './components/category/category.component';
import { BooksListByCategoryComponent } from './components/books-list-by-category/books-list-by-category.component';
import { BookDetailComponent } from './components/book-detail/book-detail.component';
import { AuthorsListComponent } from './components/authors-list/authors-list.component';
import { AuthorDetailComponent } from './components/author-detail/author-detail.component';
import { CartComponent } from './components/cart/cart.component';
import { MyAccountComponent } from './components/my-account/my-account.component';
import { WishlistComponent } from './components/wishlist/wishlist.component';
import { AddressComponent } from './components/my-account/address/address.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrderReceivedComponent } from './components/order-received/order-received.component';
import { SearchListComponent } from './components/search-list/search-list.component';

const routes: Routes = [
  {
    path: "",
    component: LayoutComponent,
    children: [
      {
        path:"",
        component: HomeComponent
      },
      {
        path:"category",
        component:CategoryComponent
      },
      {
        path:"books-list-by-category/:value",
        component:BooksListByCategoryComponent
      },
      {
        path:"book-detail/:value",
        component:BookDetailComponent
      },
      {
        path:"authors-list",
        component:AuthorsListComponent
      },
      {
        path: "author-detail/:value",
        component : AuthorDetailComponent
      },
      {
        path: "my-account",
        component : MyAccountComponent
      },
      {
        path: "my-account/address/:value",
        component : AddressComponent
      },
      {
        path: "wishlist",
        component : WishlistComponent
      },
      {
        path: "cart",
        component : CartComponent
      },
      {
        path:"checkout",
        component:CheckoutComponent
      },
      {
        path:"order-received/:value",
        component:OrderReceivedComponent
      },
      {
        path:"search-list/:search/:anotherParam",
        component:SearchListComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
