import { Component, OnInit, Inject } from '@angular/core'
import { FormGroup, FormBuilder, AbstractControl, FormControl, ValidatorFn } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import {Observable, map, startWith } from 'rxjs'
import { UserShrinkedDetailsToSelectDTO } from 'src/app/models/user-shrinked-details-to-select'
import { SubjectDetailsToSelectDTO } from 'src/app/models/subject-details-to-select'
import { ClassDetailsToSelectDTO } from '../../../../models/class-details-to-select'

@Component({
  selector: 'app-lesson-dialog',
  templateUrl: './lesson-dialog.component.html',
  styleUrls: ['./lesson-dialog.component.css']
})
export class LessonDialogComponent implements OnInit {
  public myForm!: FormGroup

  public subjects: SubjectDetailsToSelectDTO[]
  public teachers: UserShrinkedDetailsToSelectDTO[]
  public classrooms: ClassDetailsToSelectDTO[]

  public filteredSubjects!: Observable<SubjectDetailsToSelectDTO[]>
  public filteredTeachers!: Observable<UserShrinkedDetailsToSelectDTO[]>
  public filteredClassrooms!: Observable<ClassDetailsToSelectDTO[]>

  private autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null
    }
  }

  constructor(public matDialogRef: MatDialogRef<LessonDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                title: string, 
                subjects: SubjectDetailsToSelectDTO[],
                selectedSubjectId: number | undefined,
                teachers: UserShrinkedDetailsToSelectDTO[],
                selectedTeacherId: number | undefined,
                classrooms: ClassDetailsToSelectDTO[],
                selectedClassroomId: number | undefined
              },
              private formBuilder: FormBuilder) {
    this.subjects = data.subjects
    this.teachers = data.teachers
    this.classrooms = data.classrooms

    let subjectInitialValue = data.selectedSubjectId === undefined ? '' : data.subjects.find(s => s.id === data.selectedSubjectId)
    let teacherInitialValue = data.selectedTeacherId === undefined ? '' : data.teachers.find(s => s.id === data.selectedTeacherId)
    let classroomInitialValue = data.selectedClassroomId === undefined ? '' : data.classrooms.find(s => s.id === data.selectedClassroomId)

    this.myForm = this.formBuilder.group({
      subject: [subjectInitialValue, this.autocompleteObjectValidator()],
      teacher: [teacherInitialValue, this.autocompleteObjectValidator()],
      classroom: [classroomInitialValue, this.autocompleteObjectValidator()]
    })

    this.filteredSubjects = this.subject.valueChanges.pipe(
      startWith(''),
      map((value: any) => {
        if (value) {
          if (typeof value === 'string') return this.filterSubjects(value)
          else return this.filterSubjects(value.name)
        }
        else return this.subjects
      }),
    )

    this.filteredTeachers = this.teacher.valueChanges.pipe(
      startWith(''),
      map((value: any) => {
        if (value) {
          if (typeof value === 'string') return this.filterTeachers(value)
          else return this.filterTeachers(value.firstName + ' ' + value.lastName)
        }
        else return this.teachers
      }),
    )

    this.filteredClassrooms = this.classroom.valueChanges.pipe(
      startWith(''),
      map((value: any) => {
        if (value) {
          if (typeof value === 'string') return this.filterClassrooms(value)
          else return this.filterClassrooms(value.id)
        }
        else return this.classrooms
      }),
    )
  }

  ngOnInit(): void {
  }

  public get subject(): FormControl {
    return this.myForm.get('subject')! as FormControl
  }

  public get teacher(): FormControl {
    return this.myForm.get('teacher')! as FormControl
  }

  public get classroom(): FormControl {
    return this.myForm.get('classroom')! as FormControl
  }

  private filterSubjects(name: string): SubjectDetailsToSelectDTO[] {
    const filteredName = name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase()
    return this.subjects.filter((subject: SubjectDetailsToSelectDTO) => subject.name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase().startsWith(filteredName))
  }

  private filterTeachers(name: string): UserShrinkedDetailsToSelectDTO[] {
    const filteredName = name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase()
    return this.teachers.filter((teacher: UserShrinkedDetailsToSelectDTO) => {
      let teacherFullName = teacher.firstName + ' ' +  teacher.lastName
      return teacherFullName.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase().startsWith(filteredName)
    })
  }

  private filterClassrooms(name: string): ClassDetailsToSelectDTO[] {
    const filteredName = name
    return this.classrooms.filter((classroom: ClassDetailsToSelectDTO) => classroom.id.toString().startsWith(filteredName))
  }

  public getSubjectName(subject: SubjectDetailsToSelectDTO): string {
    return subject ? subject.name : ''
  }

  public getTeacherName(teacher: UserShrinkedDetailsToSelectDTO): string {
    return teacher ? teacher.firstName + ' ' + teacher.lastName : ''
  }

  public getClassroomName(classroom: ClassDetailsToSelectDTO): string {
    return classroom ? classroom.id.toString() : ''
  }

  public onSubmit(): void {
    this.matDialogRef.close(this.myForm.value)
  }
}