import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable, tap } from 'rxjs'

import { environment } from 'src/environments/environment'
import { SubjectMarksDetailsToSelectDTO } from './../models/subject-marks-details-to-select'
import { MarkDetailsToSelectDTO } from './../models/marks-details-to-select'
import { MarkSemesterEnum } from './../models/mark-semester.enum'
import { MarkTypeEnum } from './../models/mark-type.enum'
import { MarkCategoryEnum } from '../models/mark-category.enum'
import { MarkDetailsToInsertDTO } from './../models/mark-details-to-insert'
import { MarkDetailsToUpdateDTO } from './../models/mark-details-to-update'
import { MarksStatisticalDataToSelectDTO } from './../models/marks-statistical-data-to-select'

@Injectable({
  providedIn: 'root'
})
export class MarkService {

  constructor(private httpService: HttpClient) { }

  public selectMarks(pupilId: number): Observable<SubjectMarksDetailsToSelectDTO[]> {
    return this.httpService.get<SubjectMarksDetailsToSelectDTO[]>(environment.apiURL + `/Mark?pupilId=${pupilId}`, { responseType: 'json' }).pipe(
      tap((response: SubjectMarksDetailsToSelectDTO[]) => {
        response.forEach((subjectMarks: SubjectMarksDetailsToSelectDTO) => {
          subjectMarks.marks.forEach((mark: MarkDetailsToSelectDTO) => {
            if (mark.semester !== undefined) mark.semester = MarkSemesterEnum[mark.semester.toString() as keyof typeof MarkSemesterEnum]
            mark.type = MarkTypeEnum[mark.type.toString() as keyof typeof MarkTypeEnum]
            if (mark.category !== undefined) mark.category = MarkCategoryEnum[mark.category.toString() as keyof typeof MarkCategoryEnum]
          })
        })
      })
    )
  }

  public insertMark(markDetailsToInsertDTO: MarkDetailsToInsertDTO): Observable<number> {
    return this.httpService.post<number>(environment.apiURL + '/Mark', markDetailsToInsertDTO)
  }

  public updateMark(markDetailsToUpdateDTO: MarkDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/Mark', markDetailsToUpdateDTO)
  }

  public deleteMark(markId: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Mark?markId=${markId}`)
  }

  public selectPartialMarksStatisticalData(pupilId: number, subjectId: number): Observable<MarksStatisticalDataToSelectDTO[]> {
    return this.httpService.get<MarksStatisticalDataToSelectDTO[]>(environment.apiURL + `/Mark/Partial/Statistics?pupilId=${pupilId}&subjectId=${subjectId}`)
  }
}