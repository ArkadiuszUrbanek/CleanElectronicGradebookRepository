import { Router } from '@angular/router'
import { Component, OnInit } from '@angular/core'

import { MatDialog } from '@angular/material/dialog'

import { PagedResponse } from './../../models/paged-response'
import { UserRoleEnum } from '../../models/user-role.enum'
import { OrderEnum } from '../../models/order.enum'
import { AuthService } from './../../services/auth.service'
import { SurveyService } from './../../services/survey.service'
import { SurveyDetailsToSelectDTO } from 'src/app/models/survey-details-to-select'
import { SurveySortablePropertiesEnum } from '../../models/survey-sortable-properties.enum'
import { SurveyDialogComponent } from './components/survey-dialog/survey-dialog.component'
import { SurveyExtendedDetailsToSelectDTO } from './../../models/survey-extended-details-to-select'

@Component({
  selector: 'app-surveys-page',
  templateUrl: './surveys-page.component.html',
  styleUrls: ['./surveys-page.component.css']
})
export class SurveysPageComponent implements OnInit {
  public OrderEnum: typeof OrderEnum = OrderEnum
  public SurveySortablePropertiesEnum: typeof SurveySortablePropertiesEnum = SurveySortablePropertiesEnum

  public order = OrderEnum.Descending
  public sortBy = SurveySortablePropertiesEnum.CreationDate

  public currentPageNumber: number = 1
  public readonly pageSize: number = 10
  public totalNumberOfPages: number = 0

  public surveys:  SurveyDetailsToSelectDTO[] = []

  public userId: number
  public userRole: UserRoleEnum
  public RoleEnum: typeof UserRoleEnum = UserRoleEnum

  constructor(private surveyService: SurveyService,
              private authService: AuthService,
              private matDialog: MatDialog,
              private router: Router) {
    this.userRole = this.authService.getUserRole()
    this.userId = this.authService.getUserId()
  }

  public ngOnInit(): void {
    this.getSurveys()
  }

  private getSurveys(): void {
    this.surveyService.selectSurveys(this.currentPageNumber, this.pageSize, this.sortBy, this.order).subscribe({
      next: (value: PagedResponse<SurveyDetailsToSelectDTO>) => {
        this.surveys = value.payload
        this.totalNumberOfPages = value.totalNumberOfPages
      },
      error: (err: any) => {}
    })
  }

  public switchPage(pageNumber: number): void {
    if(pageNumber < 1 || pageNumber > this.totalNumberOfPages) return
    this.currentPageNumber = pageNumber
    this.getSurveys()
  }

  public sort(sortBy: SurveySortablePropertiesEnum): void {
    if(this.sortBy === sortBy) this.switchOrder()
    else {
      this.sortBy = sortBy
      this.order = OrderEnum.Descending
    }
    this.getSurveys()
  }

  private switchOrder(): void {
    this.order === OrderEnum.Descending ? this.order = OrderEnum.Ascending : this.order = OrderEnum.Descending
  }

  public openDialog(surveyId: number): void {
    this.surveyService.selectSurveyById(surveyId).subscribe({
      next: (response: SurveyExtendedDetailsToSelectDTO) => {
        response.questions.forEach(q => q.answers.sort((a1, a2) => a1.number - a2.number))
        response.questions.sort((q1, q2) => q1.number - q2.number)

        let dialogRef = this.matDialog.open(SurveyDialogComponent, { data: response, width: '50vw', height: '50vh'})
        
        dialogRef.afterClosed().subscribe({
          next: (value: number[]) => {
            if (value !== undefined) 
              this.surveyService.fillSurvey(surveyId, value).subscribe({
                next: (value: any) => {
                  this.getSurveys()
                },
                error: (err: any) => {}
              })
          },
          error: (err: any) => {}
        })
      },
      error: (err: any) => {}
    })
  }

  public viewResults(surveyId: number): void {
    this.router.navigate([`survey/${surveyId}/results`])
  }
}
