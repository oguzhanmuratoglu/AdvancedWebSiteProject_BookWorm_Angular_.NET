export class FilterRequestModel 
{
    authorIds:number[]=[];
    languages:string[]=[];
    reviewRatings:number[]=[];
    categoryId:number =0;
    pageSize:number=20;
    pageNumber:number=1;
    sortFilter:string="default";
    searchTerm:string="";
}