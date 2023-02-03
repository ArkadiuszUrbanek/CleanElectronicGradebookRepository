import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'

import { Observable } from 'rxjs'
import { tap } from 'rxjs/operators'

import { environment } from 'src/environments/environment'
import { AttendanceTypeEnum } from '../models/attendance-type.enum'
import { PupilWeeklyAttendanceDetailsToSelectDTO } from './../models/pupil-weekly-attendance-details-to-select'
import { WeeklyAttendanceDetailsToSelectDTO } from './../models/weekly-attendance-details-to-select'
import { AttendanceDetailsToInsertDTO } from './../models/attendance-details-to-insert'
import { AttendanceDetailsToUpdateDTO } from './../models/attendance-details-to-update'
import { AttendanceStatisticalDataToSelectDTO } from './../models/attendance-statistical-data-to-select'

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  constructor(private httpService: HttpClient) { }

  public selectWeeklyAttendances(clientDate: Date, classId?: number): Observable<WeeklyAttendanceDetailsToSelectDTO> {
    const year: string = String(clientDate.getFullYear())
    const month: string = String(clientDate.getMonth() + 1).padStart(2, '0')
    const day: string = String(clientDate.getDate()).padStart(2, '0')

    let url = `${environment.apiURL}/Attendance?clientDate=${year}-${month}-${day}`
    if (classId !== undefined) url += `&classId=${classId}`

    return this.httpService.get<WeeklyAttendanceDetailsToSelectDTO>(url, { responseType: 'json' }).pipe(
      tap((response: WeeklyAttendanceDetailsToSelectDTO) => {
        response.pupilsWeeklyAttendances.forEach((p: PupilWeeklyAttendanceDetailsToSelectDTO) => {
          for (let workday in p.dailyAttendances) {
            for (let teachingHourId in p.dailyAttendances[workday]) {
              p.dailyAttendances[workday][teachingHourId].type = AttendanceTypeEnum[p.dailyAttendances[workday][teachingHourId].type.toString() as keyof typeof AttendanceTypeEnum]
            }
          }
        })
      })
    )
  }

  public insertAttendance(attendanceDetailsToInsertDTO: AttendanceDetailsToInsertDTO): Observable<number> {
    //const year: string = String(attendanceDetailsToInsertDTO.date.getFullYear())
    //const month: string = String(attendanceDetailsToInsertDTO.date.getMonth() + 1).padStart(2, '0')
    //const day: string = String(attendanceDetailsToInsertDTO.date.getDate()).padStart(2, '0')

    return this.httpService.post<number>(environment.apiURL + '/Attendance', {
      //date: `${year}-${month}-${day}`,
      date: attendanceDetailsToInsertDTO.date,
      teachingHourId: attendanceDetailsToInsertDTO.teachingHourId,
      subjectId: attendanceDetailsToInsertDTO.subjectId,
      pupilId: attendanceDetailsToInsertDTO.pupilId,
      issuerId: attendanceDetailsToInsertDTO.issuerId,
      type: AttendanceTypeEnum[attendanceDetailsToInsertDTO.type]
    })
  }

  public updateAttendance(attendanceDetailsToUpdateDTO: AttendanceDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/Attendance', attendanceDetailsToUpdateDTO)
  }

  public deleteAttendance(attendanceId: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Attendance?attendanceId=${attendanceId}`)
  }

  public selectAttendanceStatisticalData(pupilId: number): Observable<AttendanceStatisticalDataToSelectDTO[]> {
    return this.httpService.get<AttendanceStatisticalDataToSelectDTO[]>(environment.apiURL + `/Attendance/Statistics?pupilId=${pupilId}`)
  }
}