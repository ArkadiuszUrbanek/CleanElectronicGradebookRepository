import { Injectable } from '@angular/core'

import { Observable, Subject } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private subject = new Subject<any>();  
  
  public showDialog(message: string, onConfirm: () => void, onCancel: () => void): void {
    this.subject.next(
      {    
        message: message,  
        onConfirm: onConfirm,
        onCancel: onCancel   
      }  
    )  
  }  

  public getSubjectAsObservable(): Observable<any> {  
      return this.subject.asObservable() 
  }  
}
