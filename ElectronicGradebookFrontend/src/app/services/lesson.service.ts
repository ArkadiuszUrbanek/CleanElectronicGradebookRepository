import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable, tap } from 'rxjs'

import { environment } from 'src/environments/environment'
import { WorkdayEnum } from '../models/workday.enum'
import { LessonDetailsToSelectDTO } from './../models/lesson-details-to-select'
import { WorkdayDetailsToSelectDTO } from './../models/workday-details-to-select'
import { TeachingHourDetailsToSelectDTO } from './../models/teaching-hour-details-to-select'
import { WeeklyTimetableDetailsToSelectDTO } from './../models/weekly-timetable-details-to-select'
import { LessonDetailsToInsertDTO } from './../models/lesson-details-to-insert'
import { LessonDetailsToUpdateDTO } from './../models/lesson-details-to-update'
import { LessonStatusEnum } from '../models/lesson-status.enum'
import { LessonExceptionDetailsToInsertDTO } from './../models/lesson-exception-details-to-insert'
import { LessonExceptionDetailsToUpdateDTO } from './../models/lesson-exception-details-to-update'
import { UserGenderEnum } from '../models/user-gender.enum'

@Injectable({
  providedIn: 'root'
})
export class LessonService {

  constructor(private httpService: HttpClient) { }

  public selectLessonPlan(clientDate: Date, classId: number): Observable<WeeklyTimetableDetailsToSelectDTO> {
    const year: string = String(clientDate.getFullYear())
    const month: string = String(clientDate.getMonth() + 1).padStart(2, '0')
    const day: string = String(clientDate.getDate()).padStart(2, '0')

    return this.httpService.get<WeeklyTimetableDetailsToSelectDTO>(environment.apiURL + `/Lesson/Plan?clientDate=${year}-${month}-${day}&classId=${classId}`, { responseType: 'json' })
      .pipe(
        tap((response: WeeklyTimetableDetailsToSelectDTO) => {
          response.workdays.forEach((w: WorkdayDetailsToSelectDTO) => {
            w.workday = WorkdayEnum[w.workday.toString() as keyof typeof WorkdayEnum]
            w.date = new Date(w.date.toString())
          })
          response.teachingHours.forEach((th : TeachingHourDetailsToSelectDTO) => {
            let dateTime: Date = new Date()
            dateTime.setHours(Number(th.startTime.toString().slice(0, 2)))
            dateTime.setMinutes(Number(th.startTime.toString().slice(3, 5)))
            dateTime.setSeconds(0)
            dateTime.setMilliseconds(0)
            th.startTime = dateTime
            th.endTime = new Date(dateTime)
            th.endTime.setMinutes(th.endTime.getMinutes() + 45)
          })
          response.lessons.forEach((l: LessonDetailsToSelectDTO) => {
            l.workday = WorkdayEnum[l.workday.toString() as keyof typeof WorkdayEnum]
            l.status = LessonStatusEnum[l.status.toString() as keyof typeof LessonStatusEnum]
            l.teacher.gender = UserGenderEnum[l.teacher.gender.toString() as keyof typeof UserGenderEnum]
            if (l.substituteTeacher !== undefined) l.substituteTeacher.gender = UserGenderEnum[l.substituteTeacher.gender.toString() as keyof typeof UserGenderEnum]
          })
        })
      )
  }

  public insertLesson(lessonDetailsToInsertDTO: LessonDetailsToInsertDTO): Observable<any> {
    return this.httpService.post(environment.apiURL + '/Lesson', lessonDetailsToInsertDTO)
  }

  public updateLesson(lessonDetailsToUpdateDTO: LessonDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/Lesson', lessonDetailsToUpdateDTO)
  }

  public deleteLesson(lessonId: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Lesson?id=${lessonId}`)
  }

  public insertLessonException(lessonExceptionDetailsToInsertDTO: LessonExceptionDetailsToInsertDTO): Observable<any> {
    const year: string = String(lessonExceptionDetailsToInsertDTO.date.getFullYear())
    const month: string = String(lessonExceptionDetailsToInsertDTO.date.getMonth() + 1).padStart(2, '0')
    const day: string = String(lessonExceptionDetailsToInsertDTO.date.getDate()).padStart(2, '0')

    return this.httpService.post(environment.apiURL + '/Lesson/Exception', {
      date: `${year}-${month}-${day}`,
      lessonId: lessonExceptionDetailsToInsertDTO.lessonId,
      teacherId: lessonExceptionDetailsToInsertDTO.teacherId,
      status: LessonStatusEnum[lessonExceptionDetailsToInsertDTO.status]
    })
  }

  public updateLessonException(lessonExceptionDetailsToUpdateDTO: LessonExceptionDetailsToUpdateDTO): Observable<any> {
    const year: string = String(lessonExceptionDetailsToUpdateDTO.date.getFullYear())
    const month: string = String(lessonExceptionDetailsToUpdateDTO.date.getMonth() + 1).padStart(2, '0')
    const day: string = String(lessonExceptionDetailsToUpdateDTO.date.getDate()).padStart(2, '0')

    return this.httpService.patch(environment.apiURL + '/Lesson/Exception', {
      date: `${year}-${month}-${day}`,
      lessonId: lessonExceptionDetailsToUpdateDTO.lessonId,
      teacherId: lessonExceptionDetailsToUpdateDTO.teacherId,
      status: LessonStatusEnum[lessonExceptionDetailsToUpdateDTO.status]
    })
  }

  public deleteLessonException(date: Date, lessonId: number): Observable<any> {
    const year: string = String(date.getFullYear())
    const month: string = String(date.getMonth() + 1).padStart(2, '0')
    const day: string = String(date.getDate()).padStart(2, '0')

    return this.httpService.delete(environment.apiURL + `/Lesson/Exception?date=${year}-${month}-${day}&lessonId=${lessonId}`)
  }
}