import { Component, OnInit, Inject } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl, ValidatorFn } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { LessonStatusEnum } from './../../../../models/lesson-status.enum'
import { UserShrinkedDetailsToSelectDTO } from 'src/app/models/user-shrinked-details-to-select'

@Component({
  selector: 'app-lesson-exception-dialog',
  templateUrl: './lesson-exception-dialog.component.html',
  styleUrls: ['./lesson-exception-dialog.component.css']
})
export class LessonExceptionDialogComponent implements OnInit {
  public myForm!: FormGroup
  public LessonStatusEnum: typeof LessonStatusEnum  = LessonStatusEnum
  public selectedStatus: LessonStatusEnum
  public filteredTeachers: UserShrinkedDetailsToSelectDTO[]

  private autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null
    }
  }

  constructor(public matDialogRef: MatDialogRef<LessonExceptionDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                lessonStatus: LessonStatusEnum,
                teachers: UserShrinkedDetailsToSelectDTO[],
                substituteTeacherId: number | undefined
              },
              private formBuilder: FormBuilder) {
    this.selectedStatus = data.lessonStatus
    this.filteredTeachers = data.teachers

    this.myForm = this.formBuilder.group({
      lessonStatus: [data.lessonStatus, Validators.required],
    })

    if (data.lessonStatus === LessonStatusEnum.Substitution) {
      const teacherInitialValue = this.data.substituteTeacherId === undefined ? '' : data.teachers.find(s => s.id === this.data.substituteTeacherId)
      this.myForm.addControl('teacherSubstitutor', this.formBuilder.control(teacherInitialValue, this.autocompleteObjectValidator()))

      this.teacherSubstitutor.valueChanges.subscribe(
        (value: any) => {
          if (value) {
            if (typeof value === 'string') this.filteredTeachers = this.filterTeachers(value)
            else this.filteredTeachers = this.filterTeachers(value.firstName + ' ' + value.lastName)
          }
          else this.filteredTeachers = this.data.teachers
        }
      )
    }

    this.lessonStatus.valueChanges.subscribe((status: LessonStatusEnum) => {
      if (status === LessonStatusEnum.Substitution) {
        const teacherInitialValue: any = this.data.substituteTeacherId === undefined ? '' : this.data.teachers.find(s => s.id === this.data.substituteTeacherId)
        this.myForm.addControl('teacherSubstitutor', this.formBuilder.control(teacherInitialValue, this.autocompleteObjectValidator()))
        this.filteredTeachers = data.teachers

        this.teacherSubstitutor.valueChanges.subscribe(
          (value: any) => {
            if (value) {
              if (typeof value === 'string') this.filteredTeachers = this.filterTeachers(value)
              else this.filteredTeachers = this.filterTeachers(value.firstName + ' ' + value.lastName)
            }
            else this.filteredTeachers = this.data.teachers
          }
          )
        }
        else this.myForm.removeControl('teacherSubstitutor')

        this.selectedStatus = status
      })
  }

  public ngOnInit(): void {
  }

  public get lessonStatus(): FormControl {
    return this.myForm.get('lessonStatus')! as FormControl
  }

  public get teacherSubstitutor(): FormControl {
    return this.myForm.get('teacherSubstitutor')! as FormControl
  }

  private filterTeachers(name: string): UserShrinkedDetailsToSelectDTO[] {
    const filteredName = name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase()
    return this.data.teachers.filter((teacher: UserShrinkedDetailsToSelectDTO) => {
      let teacherFullName = teacher.firstName + ' ' +  teacher.lastName
      return teacherFullName.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase().startsWith(filteredName)
    })
  }
  
  public getTeacherName(teacher: UserShrinkedDetailsToSelectDTO): string {
    return teacher ? teacher.firstName + ' ' + teacher.lastName : ''
  }

  public onSubmit(): void {
    this.matDialogRef.close(this.myForm.value)
  }
}
