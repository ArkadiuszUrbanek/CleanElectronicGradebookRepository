import { SurveyStatisticalDataToSelectDTO } from './../../models/survey-statistical-data-to-select'
import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'

import { SurveyService } from './../../services/survey.service'

@Component({
  selector: 'app-survey-results',
  templateUrl: './survey-results-page.component.html',
  styleUrls: ['./survey-results-page.component.css']
})
export class SurveyResultsPageComponent implements OnInit {
  private surveyId: number
  public surveyStatisticalData!: SurveyStatisticalDataToSelectDTO
  public questionsChartsTypes: ('pie' | 'bar')[] = []
  public questionsAnswersContents: string[][] = []
  public questionsAnswersTimesSelected: number[][] = []

  constructor(private acitvatedRoute: ActivatedRoute,
              private surveyService: SurveyService) {
    this.surveyId = Number(this.acitvatedRoute.snapshot.paramMap.get('id'))
  }

  public ngOnInit(): void {
    this.surveyService.getSurveyStatisticalData(this.surveyId).subscribe({
      next: (response: SurveyStatisticalDataToSelectDTO) => {
        response.questions.forEach(q => q.answers.sort((a1, a2) => a1.number - a2.number))
        response.questions.sort((q1, q2) => q1.number - q2.number)

        this.surveyStatisticalData = response
        this.questionsChartsTypes = new Array<('pie' | 'bar')>(response.questions.length).fill('pie')
        
        for(let i = 0; i < response.questions.length; i++) {
          let tempQuestionsAnswersContents: string[] = []
          let tempQuestionsAnswersTimesSelected: number[] = []

          for(let j = 0; j < response.questions[i].answers.length; j++) {
            tempQuestionsAnswersContents.push(response.questions[i].answers[j].contents)
            tempQuestionsAnswersTimesSelected.push(response.questions[i].answers[j].timesSelected)
          }

          this.questionsAnswersContents.push(tempQuestionsAnswersContents)
          this.questionsAnswersTimesSelected.push(tempQuestionsAnswersTimesSelected)
        }
      },
      error: (value: any) => {}
    })
  }

  public changeChartType(requestedChartType: 'pie' | 'bar', index: number): void {
    this.questionsChartsTypes[index] = requestedChartType
  }

}