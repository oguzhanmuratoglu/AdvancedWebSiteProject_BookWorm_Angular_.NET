export class CheckoutBillingAddressModel
{
    id:number=0;
    userId:number=0;
    orderId:number=0;
    addressType:string="Billing";
    contactName:string="";
    contactEmail:string="";
    contactPhoneNumber:string="";
    city:string="";
    country:string="";
    zipCode:string="";
    addressDescription:string="";
}