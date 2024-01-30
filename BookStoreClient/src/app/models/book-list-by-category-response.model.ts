export class BookListByCategoryResponseModel
{
    data: any[]=[];
    categoryName : string="";
    totalPageCount: number=0;
    isLastPage:boolean=false;
    isFirstPage:boolean=true;
}