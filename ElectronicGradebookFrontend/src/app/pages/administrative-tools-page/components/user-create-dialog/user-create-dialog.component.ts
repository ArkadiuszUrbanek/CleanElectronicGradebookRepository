import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms'
import { Component } from '@angular/core'

import { MatDialogRef } from '@angular/material/dialog'

import { UserRoleEnum } from './../../../../models/user-role.enum'
import { UserGenderEnum } from './../../../../models/user-gender.enum'

@Component({
  selector: 'app-user-create-dialog',
  templateUrl: './user-create-dialog.component.html',
  styleUrls: ['./user-create-dialog.component.css']
})
export class UserCreateDialogComponent {
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  public UserGenderEnum: typeof UserGenderEnum = UserGenderEnum

  public myForm: FormGroup
  public isPasswordVisible: boolean = false

  constructor(public matDialogRef: MatDialogRef<UserCreateDialogComponent>,
              private formBuilder: FormBuilder) {
    this.myForm = this.formBuilder.group({
      'firstName': ['', [Validators.required, Validators.maxLength(20)]],
      'lastName': ['', [Validators.required, Validators.maxLength(30)]],
      'gender': '',
      'role': '',
      'email': ['', [Validators.required, Validators.maxLength(50), Validators.email]],
      'password': ['', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-~\\\\`!@#$%^&*()[+={}:;"<>,.?/_|\\]\'\\ ])[A-Za-z\d\W].{7,}$')]]
    })

    this.role.valueChanges.subscribe((value: UserRoleEnum) => {
      if (value === UserRoleEnum.Pupil) {
        this.myForm.removeControl('contactNumber')
        this.myForm.removeControl('contactEmail')
        this.myForm.addControl('secondName', new FormControl('', Validators.maxLength(20)))
        this.myForm.addControl('birthDate', new FormControl(''))
        return
      }

      if (value === UserRoleEnum.Teacher) {
        this.myForm.removeControl('secondName')
        this.myForm.removeControl('birthDate')
        this.myForm.addControl('contactNumber', new FormControl('', Validators.pattern("^[0-9]{9}$")))
        this.contactNumber.valueChanges.subscribe((value: string) => {
          if (value && !value.match(/^[0-9]{1,9}$/)) this.contactNumber.setValue(value.slice(0, -1))
        })
        this.myForm.addControl('contactEmail', new FormControl('', [Validators.maxLength(50), Validators.email]))
        return
      }
    })
  }

  public get firstName(): FormControl {
    return this.myForm.get('firstName') as FormControl
  }

  public get secondName(): FormControl {
    return this.myForm.get('secondName') as FormControl
  }

  public get lastName(): FormControl {
    return this.myForm.get('lastName') as FormControl
  }

  public get gender(): FormControl {
    return this.myForm.get('gender') as FormControl
  }

  public get birthDate(): FormControl {
    return this.myForm.get('birthDate') as FormControl
  }

  public get role(): FormControl {
    return this.myForm.get('role') as FormControl
  }

  public get contactNumber(): FormControl {
    return this.myForm.get('contactNumber') as FormControl
  }

  public get contactEmail(): FormControl {
    return this.myForm.get('contactEmail') as FormControl
  }

  public get email(): FormControl {
    return this.myForm.get('email') as FormControl
  }

  public get password(): FormControl {
    return this.myForm.get('password') as FormControl
  }

  public onSubmit(): void {
    if (this.myForm.value.role === UserRoleEnum.Teacher) {
      if (!this.myForm.value.contactNumber) delete this.myForm.value["contactNumber"]
      if (!this.myForm.value.contactEmail) delete this.myForm.value["contactEmail"]
    }

    if (this.myForm.value.role === UserRoleEnum.Pupil) {
      if (!this.myForm.value.secondName) delete this.myForm.value["secondName"]
      if (!this.myForm.value.birthDate) delete this.myForm.value["birthDate"]
    }
    
    this.matDialogRef.close(this.myForm.value)
  }
}
