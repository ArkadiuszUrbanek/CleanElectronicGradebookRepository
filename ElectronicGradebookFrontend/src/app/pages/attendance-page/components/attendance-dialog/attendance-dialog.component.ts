import { FormBuilder, FormGroup, AbstractControl, ValidatorFn, Validators, FormControl } from '@angular/forms'
import { Component, OnInit, Inject } from '@angular/core'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { AttendanceTypeEnum } from 'src/app/models/attendance-type.enum'
import { SubjectDetailsToSelectDTO } from 'src/app/models/subject-details-to-select'

@Component({
  selector: 'app-attendance-dialog',
  templateUrl: './attendance-dialog.component.html',
  styleUrls: ['./attendance-dialog.component.css']
})
export class AttendanceDialogComponent implements OnInit {
  public AttendanceTypeEnum: typeof AttendanceTypeEnum = AttendanceTypeEnum
  public myForm: FormGroup

  public filteredSubjects: SubjectDetailsToSelectDTO[] = []

  private autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null
    }
  }

  constructor(public matDialogRef: MatDialogRef<AttendanceDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                title: string,
                subjects: SubjectDetailsToSelectDTO[]
                selectedSubjectId?: number,
                selectedType?: AttendanceTypeEnum
              },
              private formBuilder: FormBuilder) {
    this.filteredSubjects = this.data.subjects
    
    let subjectInitialValue = data.selectedSubjectId === undefined ? '' : data.subjects.find(s => s.id === data.selectedSubjectId)
    
    this.myForm = this.formBuilder.group({
      subject: [subjectInitialValue, this.autocompleteObjectValidator()],
      type: [data.selectedType, Validators.required]
    })

    this.subject.valueChanges.subscribe(
      (value: any) => {
        if (value) {
          if (typeof value === 'string') return this.filteredSubjects = this.filterSubjects(value)
          else return this.filteredSubjects = this.filterSubjects(value.name)
        }
        else return this.filteredSubjects = this.data.subjects
      }
    )

  }

  public ngOnInit(): void {
  }

  public get subject(): FormControl {
    return this.myForm.get("subject")! as FormControl
  }

  public get type(): FormControl {
    return this.myForm.get("type")! as FormControl
  }

  private filterSubjects(name: string): SubjectDetailsToSelectDTO[] {
    const filteredName = name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase()
    return this.data.subjects.filter((subject: SubjectDetailsToSelectDTO) => subject.name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase().startsWith(filteredName))
  }

  public getSubjectName(subject: SubjectDetailsToSelectDTO): string {
    return subject ? subject.name : ''
  }

  public onSubmit(): void {
    this.matDialogRef.close(this.myForm.value)
  }
}
