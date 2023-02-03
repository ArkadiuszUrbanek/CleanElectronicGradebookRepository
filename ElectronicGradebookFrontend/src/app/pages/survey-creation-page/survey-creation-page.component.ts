import { Router } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import { FormArray, FormBuilder, FormGroup, AbstractControl, FormControl, Validators } from '@angular/forms'

import { AuthService } from './../../services/auth.service'
import { SurveyService } from './../../services/survey.service'
import { QuestionTypeEnum } from './../../models/question-type.enum'
import { UserRoleEnum } from '../../models/user-role.enum'
import { SurveyDetailsToInsertDTO } from './../../models/survey-details-to-insert'
import { QuestionDetailsToInsertDTO } from './../../models/question-details-to-insert'

@Component({
  selector: 'app-survey-creation-page',
  templateUrl: './survey-creation-page.component.html',
  styleUrls: ['./survey-creation-page.component.css']
})
export class SurveyCreationPageComponent implements OnInit {
  public RoleEnum: typeof UserRoleEnum = UserRoleEnum
  public QuestionTypeEnum: typeof QuestionTypeEnum = QuestionTypeEnum
  public surveyFormGroup: FormGroup

  private dateValidator(control: FormControl) {
    if(!control.value || new Date(control.value) > new Date()) return null
    else return { dateValidator: { valid: false } }
  }

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private surveyService: SurveyService,
              private router: Router) {
    this.surveyFormGroup = formBuilder.group({
      name: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      description: ['', Validators.compose([Validators.required, Validators.maxLength(500)])],
      targetedRoles: ['', Validators.required],
      expirationDate: ['', Validators.compose([Validators.required, this.dateValidator])],
      questions: this.formBuilder.array([], Validators.required)
    })
  }

  public ngOnInit(): void {

  }

  public get name(): AbstractControl {
    return this.surveyFormGroup.get('name')!
  }

  public get description(): AbstractControl {
    return this.surveyFormGroup.get('description')!
  }

  public get targetedRoles(): AbstractControl {
    return this.surveyFormGroup.get('targetedRoles')!
  }

  public get expirationDate(): AbstractControl {
    return this.surveyFormGroup.get('expirationDate')!
  }

  public get questions(): FormArray {
    return this.surveyFormGroup.get('questions') as FormArray
  }

  public questionAnswers(questionIndex: number): FormArray {
     return this.questions.at(questionIndex).get('answers') as FormArray
  }

  public addAnswer(questionIndex: number): void {
    this.questionAnswers(questionIndex).push(this.formBuilder.group({
      contents: ['', Validators.compose([Validators.required, Validators.maxLength(50)])]
    }))
  }

  public removeAnswer(questionIndex: number, answerIndex: number): void {
    this.questionAnswers(questionIndex).removeAt(answerIndex)
  }

  public addSingleChoiceQuestion(): void {
    this.questions.push(this.formBuilder.group({
      contents: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
      type: [QuestionTypeEnum.SingleChoice],
      answers: this.formBuilder.array([], Validators.compose([Validators.required, Validators.minLength(2)]))
    }))
  }

  public addMultipleChoiceQuestion(): void {
    this.questions.push(this.formBuilder.group({
      contents: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
      type: [QuestionTypeEnum.MultipleChoice],
      answers: this.formBuilder.array([], Validators.compose([Validators.required, Validators.minLength(2)]))
    }))
  }

  public removeQuestion(questionIndex: number): void {
    this.questions.removeAt(questionIndex)
  }

  public onSubmit(): void {
    let surveyDetailsToInsertDTO: SurveyDetailsToInsertDTO = {
      name: this.surveyFormGroup.value.name,
      description: this.surveyFormGroup.value.description,
      authorId: this.authService.getUserId(),
      targetedRoles: this.surveyFormGroup.value.targetedRoles,
      expirationDate: new Date(this.surveyFormGroup.value.expirationDate),
      questions: this.surveyFormGroup.value.questions.map((
        question : { 
          contents: string,
          type: QuestionTypeEnum,
          answers: { contents: string }[]
        }): QuestionDetailsToInsertDTO => {
          return {
            contents: question.contents,
            type: question.type,
            answersContents: question.answers.map((answer: { contents: string }) => answer.contents)
          }
      })
    }

    this.surveyService.insertSurvey(surveyDetailsToInsertDTO).subscribe({
      next: (response: any) => {
        this.router.navigate(['surveys'])
      },
      error: (err: any) => {
        
      }
    })
  }
}