import { Injectable } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class PopupService {

  constructor
  (
    private confirmationService: ConfirmationService, 
    private messageService: MessageService,
  ) 
  {

  }

  showWarningPopup(event: Event, message:string) {
    this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: message,
        icon: 'pi pi-exclamation-triangle',
        acceptVisible: false,
        rejectVisible: false,
    });
  }
  confirmDeleted(event: Event, message:string, callBack: (res: any)=> void) {
    this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: message,
        rejectLabel: "Yes",
        acceptLabel: "No",
        icon: 'pi pi-exclamation-triangle',
        accept: (res:any) => {
        },
        reject: (res:any) => {
          callBack(res);
        }
    });
  }

  loginWarning(event: Event) {
    this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: "You cannot order products without logging in.",
        acceptVisible:false,
        rejectVisible:false,
        icon: 'pi pi-exclamation-triangle'
    });
  }

  confirmPopup(event: Event, message:string, acceptedResponse:string, rejectedResponse:string ) {
    this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: message,
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
            this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: acceptedResponse });
        },
        reject: () => {
            this.messageService.add({ severity: 'error', summary: 'Rejected', detail: rejectedResponse });
        }
    });
  }
}
