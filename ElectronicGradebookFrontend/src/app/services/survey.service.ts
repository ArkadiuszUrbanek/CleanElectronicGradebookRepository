import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable, tap } from 'rxjs'

import { environment } from 'src/environments/environment'
import { PagedResponse } from '../models/paged-response'
import { SurveyDetailsToSelectDTO } from '../models/survey-details-to-select'
import { SurveyDetailsToInsertDTO } from './../models/survey-details-to-insert'
import { SurveyExtendedDetailsToSelectDTO } from './../models/survey-extended-details-to-select'
import { QuestionDetailsToSelectDTO } from './../models/question-details-to-select'
import { SurveyStatisticalDataToSelectDTO } from './../models/survey-statistical-data-to-select'
import { QuestionStatisticalDataToSelectDTO } from './../models/question-statistical-data-to-select'
import { OrderEnum } from '../models/order.enum'
import { QuestionTypeEnum } from './../models/question-type.enum'
import { SurveySortablePropertiesEnum } from '../models/survey-sortable-properties.enum'

@Injectable({
  providedIn: 'root'
})
export class SurveyService {

  constructor(private httpService: HttpClient) { }

  public selectSurveys(pageNumber: number, pageSize: number, sortBy: SurveySortablePropertiesEnum, order: OrderEnum): Observable<PagedResponse<SurveyDetailsToSelectDTO>> {
    return this.httpService.get<PagedResponse<SurveyDetailsToSelectDTO>>(environment.apiURL + `/Survey?PageNumber=${pageNumber}&PageSize=${pageSize}&SortBy=${SurveySortablePropertiesEnum[sortBy]}&Order=${OrderEnum[order]}`, { 
      responseType: 'json' 
    })
  }

  public insertSurvey(surveyDetailsToInsertDTO: SurveyDetailsToInsertDTO): Observable<any> {
    return this.httpService.post(environment.apiURL + '/Survey', surveyDetailsToInsertDTO)
  }

  public selectSurveyById(surveyId: number): Observable<SurveyExtendedDetailsToSelectDTO> {
    return this.httpService.get<SurveyExtendedDetailsToSelectDTO>(environment.apiURL + `/Survey/${surveyId}`, { responseType: 'json' }).pipe(
      tap((response: SurveyExtendedDetailsToSelectDTO) => {
        response.questions.forEach((q: QuestionDetailsToSelectDTO) => {
          q.type = QuestionTypeEnum[q.type.toString() as keyof typeof QuestionTypeEnum]
        })
      })
    )
  }

  public fillSurvey(surveyId: number, selectedAnswersIds: number[]): Observable<any> {
    return this.httpService.patch(environment.apiURL + `/Survey/${surveyId}/Fill`, selectedAnswersIds)
  }

  public getSurveyStatisticalData(surveyId: number): Observable<SurveyStatisticalDataToSelectDTO> {
    return this.httpService.get<SurveyStatisticalDataToSelectDTO>(environment.apiURL + `/Survey/${surveyId}/Results`, { responseType: 'json' }).pipe(
      tap((response: SurveyStatisticalDataToSelectDTO) => {
        response.questions.forEach((q: QuestionStatisticalDataToSelectDTO) => {
          q.type = QuestionTypeEnum[q.type.toString() as keyof typeof QuestionTypeEnum]
        })
      })
    )
  }
}
