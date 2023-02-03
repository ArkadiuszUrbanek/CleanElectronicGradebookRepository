import { Component, OnInit, Inject } from '@angular/core'
import { FormGroup, Validators, FormControl } from '@angular/forms'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper'

import { QuestionTypeEnum } from './../../../../models/question-type.enum'
import { SurveyExtendedDetailsToSelectDTO } from './../../../../models/survey-extended-details-to-select'
import { MatCheckboxChange } from '@angular/material/checkbox'

@Component({
  selector: 'app-survey-dialog',
  templateUrl: './survey-dialog.component.html',
  styleUrls: ['./survey-dialog.component.css'],
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: {showError: true}
    }
  ]
})
export class SurveyDialogComponent implements OnInit {
  private checkboxAnswersToQuestions: Map<number, number[]> = new Map()
  public surveyFormGroup: FormGroup
  public QuestionTypeEnum: typeof QuestionTypeEnum = QuestionTypeEnum
  
  private initQuestions(): FormGroup {
    const questions: FormGroup = new FormGroup({})
    this.data.questions.forEach(q => questions.addControl(q.id.toString(), new FormControl('', [Validators.required])))
    return questions
  }

  addAnswerToQuestion(event: MatCheckboxChange, questionId: number) {
    let answersIds: number[] = []
    if (this.checkboxAnswersToQuestions.has(questionId)) {
      answersIds = this.checkboxAnswersToQuestions.get(questionId) as number[]
    }

    if (event.checked) {
      answersIds.push(+event.source.value)
    } else {
      answersIds = answersIds.filter(an => an != +event.source.value)
    }

    if(answersIds.length === 0) {
      this.surveyFormGroup.get(questionId.toString())?.setErrors({'required': true})
    } else {
      this.surveyFormGroup.get(questionId.toString())?.setErrors(null)
    }

    this.checkboxAnswersToQuestions.set(questionId, answersIds)
  }

  constructor(public matDialogRef: MatDialogRef<SurveyDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: SurveyExtendedDetailsToSelectDTO) {
    this.surveyFormGroup = this.initQuestions()
  }

  public ngOnInit(): void {
  }

  public onSubmit(): void {
    let selectedAnswersIds: number[] = []

    Object.keys(this.surveyFormGroup.value).forEach(key => {
      if (this.surveyFormGroup.value[key]) selectedAnswersIds.push(Number(this.surveyFormGroup.value[key]))
    })

    for (const value of this.checkboxAnswersToQuestions.values()) {
      selectedAnswersIds = selectedAnswersIds.concat(value)
    }

    this.matDialogRef.close(selectedAnswersIds)
  }
}