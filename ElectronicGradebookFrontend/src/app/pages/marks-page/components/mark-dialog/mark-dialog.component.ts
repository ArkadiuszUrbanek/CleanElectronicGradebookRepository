import { Component, OnInit, Inject } from '@angular/core'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { MarkTypeEnum } from '../../../../models/mark-type.enum'
import { MarkCategoryEnum } from '../../../../models/mark-category.enum'

@Component({
  selector: 'app-mark-dialog',
  templateUrl: './mark-dialog.component.html',
  styleUrls: ['./mark-dialog.component.css']
})
export class MarkDialogComponent implements OnInit {
  public MarkTypeEnum: typeof MarkTypeEnum = MarkTypeEnum
  public MarkCategoryEnum: typeof MarkCategoryEnum = MarkCategoryEnum

  public myForm: FormGroup

  constructor(public matDialogRef: MatDialogRef<MarkDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                type: MarkTypeEnum,
                title: string,
                value?: number,
                weight?: number,
                category?: MarkCategoryEnum
              },
              private formBuilder: FormBuilder) {
    this.myForm = formBuilder.group({})

    this.myForm.addControl('value', this.formBuilder.control(data.value, Validators.required))

    if (data.type === MarkTypeEnum.Partial) {
      this.myForm.addControl('category', this.formBuilder.control(data.category, Validators.required))
      this.myForm.addControl('weight', this.formBuilder.control(data.weight, Validators.required))
    }
  }

  ngOnInit(): void {
  }

  public get value(): FormControl {
    return this.myForm.get('value') as FormControl
  }

  public get category(): FormControl {
    return this.myForm.get('category') as FormControl
  }

  public get weight(): FormControl {
    return this.myForm.get('weight') as FormControl
  }

  public onSubmit(): void {
    this.matDialogRef.close(this.myForm.value)
  }
}
