import { FormControl, Validators } from '@angular/forms'
import { Component, OnInit } from '@angular/core'

import { ClassService } from './../../services/class.service'
import { TeacherDetailsToSelectDTO } from './../../models/teacher-details-to-select'
import { ClassDetailsToSelectDTO } from './../../models/class-details-to-select'
import { UserService } from './../../services/user.service'
import { AuthService } from './../../services/auth.service'
import { UserRoleEnum } from './../../models/user-role.enum'

@Component({
  selector: 'app-teachers-page',
  templateUrl: './teachers-page.component.html',
  styleUrls: ['./teachers-page.component.css']
})
export class TeachersPageComponent implements OnInit {
  public userRole: UserRoleEnum
  public userId: number
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum

  public phoneNumberControl: FormControl
  public emailAddressControl: FormControl

  public selectedClassId!: number
  public classes: ClassDetailsToSelectDTO[] = []

  public teachers: TeacherDetailsToSelectDTO[] = []
  public currentlyEditedPhoneNumber?: number = undefined
  public currentlyEditedEmailAddress?: number = undefined

  constructor(private classService: ClassService,
              private userService: UserService,
              private authService: AuthService) {

    this.phoneNumberControl = new FormControl('', Validators.pattern("^[0-9]{9}$"))
    this.emailAddressControl = new FormControl('', Validators.email)

    this.userRole = this.authService.getUserRole()
    this.userId = this.authService.getUserId()
  }

  ngOnInit(): void {
    this.classService.selectClasses().subscribe({
      next: (response: ClassDetailsToSelectDTO[]) => {
        if (response.length === 0) return

        response.sort((a: ClassDetailsToSelectDTO, b: ClassDetailsToSelectDTO) => {
          let nameA: string = a.name.toLowerCase(), nameB: string = b.name.toLowerCase()
          if (nameA < nameB) return -1
          if (nameA > nameB) return 1
          return 0
        })
        this.classes = response
        this.selectedClassId = response[0].id
        this.getTeachers()
      },
      error: (err: any) => {}
    })
  }

  public displayTeachers(classId: number): void {
    this.currentlyEditedPhoneNumber = undefined
    this.currentlyEditedEmailAddress = undefined
    this.selectedClassId = classId
    this.getTeachers()
  }

  public getTeachers(): void {
    this.userService.selectAllTeachers(this.selectedClassId).subscribe({
      next: (response: TeacherDetailsToSelectDTO[]) => {
        this.teachers = response
      },
      error: (err: any) => {}
    })
  }

  public formatPhoneNumber(phoneNumber: string): string {
    return `+48 ${phoneNumber.match(/.{3}/g)!.join(' ')}`
  }

  public enterPhoneNumberEditMode(index: number, phoneNumber?: string): void {
    this.phoneNumberControl.setValue(phoneNumber)
    this.currentlyEditedPhoneNumber = index
    this.currentlyEditedEmailAddress = undefined
  }

  public enterEmailAddressEditMode(index: number, emailAddress?: string): void {
    this.emailAddressControl.setValue(emailAddress)
    this.currentlyEditedEmailAddress = index
    this.currentlyEditedPhoneNumber = undefined
  }

  public cancelPhoneNumberEditMode(): void {
    this.currentlyEditedPhoneNumber = undefined
  }

  public cancelEmailAddressEditMode(): void {
    this.currentlyEditedEmailAddress = undefined
  }

  public savePhoneNumber(teacherId: number, contactEmail?: string): void {
    this.currentlyEditedPhoneNumber = undefined
    this.currentlyEditedEmailAddress = undefined
    
    this.userService.updateTeacherContactDetails({
      id: teacherId,
      contactEmail: contactEmail,
      contactNumber: this.phoneNumberControl.value == '' ? undefined : this.phoneNumberControl.value
    }).subscribe({
      next: (response: any) => {
        this.getTeachers()
      },
      error: (err: any) => {}
    })
  }

  public saveEmailAddress(teacherId: number, contactNumber?: string): void {
    this.currentlyEditedPhoneNumber = undefined
    this.currentlyEditedEmailAddress = undefined
    
    this.userService.updateTeacherContactDetails({
      id: teacherId,
      contactEmail: this.emailAddressControl.value == '' ? undefined : this.emailAddressControl.value,
      contactNumber: contactNumber
    }).subscribe({
      next: (response: any) => {
        this.getTeachers()
      },
      error: (err: any) => {}
    })
  }
}