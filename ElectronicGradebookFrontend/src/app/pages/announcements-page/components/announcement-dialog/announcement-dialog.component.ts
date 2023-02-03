import { Component, Inject } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { UserRoleEnum } from '../../../../models/user-role.enum'

@Component({
  selector: 'app-announcement-dialog',
  templateUrl: './announcement-dialog.component.html',
  styleUrls: ['./announcement-dialog.component.css']
})
export class AnnouncementDialogComponent {
  public myForm: FormGroup
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum

  constructor(public matDialogRef: MatDialogRef<AnnouncementDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                title: string,
                announcement: {
                  title: string,
                  contents: string,
                  authorizedRoles: UserRoleEnum[]
                }
              }, 
              private formBuilder: FormBuilder) {
    this.myForm = this.formBuilder.group({
      title: [this.data.announcement.title, Validators.compose([Validators.maxLength(50), Validators.required])],
      authorizedRoles: [this.data.announcement.authorizedRoles.length === 0 ? [UserRoleEnum.Admin] : this.data.announcement.authorizedRoles, Validators.required],
      contents: [this.data.announcement.contents, Validators.compose([Validators.maxLength(500), Validators.required])]
    })
  }

  public get title(): FormControl {
    return this.myForm.get('title')! as FormControl
  }

  public get contents(): FormControl {
    return this.myForm.get('contents')! as FormControl
  }

  public get authorizedRoles(): FormControl {
    return this.myForm.get('authorizedRoles')! as FormControl
  }

  public onSubmit(): void {
    this.matDialogRef.close(this.myForm.value)
  }
}