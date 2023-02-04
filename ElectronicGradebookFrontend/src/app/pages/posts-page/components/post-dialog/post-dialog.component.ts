import { Component, Inject } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { UserRoleEnum } from './../../../../models/user-role.enum'

@Component({
  selector: 'app-post-dialog',
  templateUrl: './post-dialog.component.html',
  styleUrls: ['./post-dialog.component.css']
})
export class PostDialogComponent {
  public myForm: FormGroup
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum

  constructor(public matDialogRef: MatDialogRef<PostDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                title: string,
                post: {
                  contents: string,
                  authorizedRoles: UserRoleEnum[]
                }
              }, 
              private formBuilder: FormBuilder) {
    this.myForm = this.formBuilder.group({
      authorizedRoles: [this.data.post.authorizedRoles.length === 0 ? [UserRoleEnum.Admin] : this.data.post.authorizedRoles, Validators.required],
      contents: [this.data.post.contents, Validators.compose([Validators.maxLength(1000), Validators.required])]
    })
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
