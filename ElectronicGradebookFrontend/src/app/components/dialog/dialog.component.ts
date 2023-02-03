import { AfterViewInit, Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core'

import { Modal } from 'bootstrap'

import { Subscription } from 'rxjs'

import { DialogService } from 'src/app/services/dialog.service'

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('dialog') dialogRef!: ElementRef<HTMLDivElement>
  private dialog!: Modal
  private dialogSubscription!: Subscription

  public message!: string
  public onConfirm!: () => void
  public onCancel!: () => void


  constructor(private dialogService: DialogService) {}  

  public ngOnInit(): any {}  

  public ngAfterViewInit(): void {
    this.dialog = new Modal(this.dialogRef.nativeElement, {
      backdrop: 'static',
      keyboard: false
    })

    this.dialogSubscription = this.dialogService.getSubjectAsObservable().subscribe((params: any) => {
      this.dialog.show()

      this.message = params.message  
      this.onConfirm = () => {
        this.dialog.hide()
        params.onConfirm()
      }
      this.onCancel = () => {
        this.dialog.hide()
        params.onCancel()
      }
    })
  }

  public ngOnDestroy(): void {
    this.dialogSubscription.unsubscribe() 
  }
}
