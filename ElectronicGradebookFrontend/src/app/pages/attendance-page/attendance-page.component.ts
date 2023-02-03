import { AttendanceChartDialogComponent } from './components/attendance-chart-dialog/attendance-chart-dialog.component'
import { AttendanceStatisticalDataToSelectDTO } from './../../models/attendance-statistical-data-to-select'
import { SubjectDetailsToSelectDTO } from 'src/app/models/subject-details-to-select'
import { Component, OnInit } from '@angular/core'

import { MatDialog } from '@angular/material/dialog'

import { UserRoleEnum } from './../../models/user-role.enum'
import { AuthService } from './../../services/auth.service'
import { ClassService } from './../../services/class.service'
import { ClassDetailsToSelectDTO } from './../../models/class-details-to-select'
import { AttendanceService } from 'src/app/services/attendance.service'
import { PupilWeeklyAttendanceDetailsToSelectDTO } from './../../models/pupil-weekly-attendance-details-to-select'
import { WorkdayEnum } from './../../models/workday.enum'
import { WorkdayDetailsToSelectDTO } from './../../models/workday-details-to-select'
import { WeeklyAttendanceDetailsToSelectDTO } from './../../models/weekly-attendance-details-to-select'
import { AttendanceTypeEnum } from 'src/app/models/attendance-type.enum'
import { SubjectService } from './../../services/subject.service'
import { AttendanceDialogComponent } from './components/attendance-dialog/attendance-dialog.component'

@Component({
  selector: 'app-attendance-page',
  templateUrl: './attendance-page.component.html',
  styleUrls: ['./attendance-page.component.css']
})
export class AttendancePageComponent implements OnInit {
  private date: Date = new Date()
  public userRole: UserRoleEnum
  public classes: ClassDetailsToSelectDTO[] = []
  public selectedClassId?: number
  public isAttendanceInitiallyLoading: boolean = true

  public workdaysDetails: WorkdayDetailsToSelectDTO[] = []
  public teachingHoursIds: number[] = []
  public pupilsWeeklyAttendancesDetails: PupilWeeklyAttendanceDetailsToSelectDTO[] = []
  
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  public WorkdayEnum: typeof WorkdayEnum = WorkdayEnum

  constructor(private authService: AuthService,
              private classService: ClassService,
              private attendanceService: AttendanceService,
              private matDialog: MatDialog,
              private subjectService: SubjectService) {
    this.userRole = this.authService.getUserRole()
  }
                
  public ngOnInit(): void {
    if (this.userRole === UserRoleEnum.Admin) {
      this.classService.selectClasses().subscribe({
        next: (response: ClassDetailsToSelectDTO[]) => {
          if (response.length === 0) return

          response.sort((a: ClassDetailsToSelectDTO, b: ClassDetailsToSelectDTO) => {
            let nameA: string = a.name.toLowerCase(), nameB: string = b.name.toLowerCase()
            if (nameA < nameB) return -1
            if (nameA > nameB) return 1
            return 0
          })

          this.classes = response
          this.selectedClassId = response[0].id
          this.getWeeklyAttendances()
          return
        },
        error: (err: any) => {}
      })
    }

    if (this.userRole === UserRoleEnum.Teacher) {
      this.classService.selectTaughtClasses().subscribe({
        next: (response: ClassDetailsToSelectDTO[]) => {
          if (response.length === 0) return

          response.sort((a: ClassDetailsToSelectDTO, b: ClassDetailsToSelectDTO) => {
            let nameA: string = a.name.toLowerCase(), nameB: string = b.name.toLowerCase()
            if (nameA < nameB) return -1
            if (nameA > nameB) return 1
            return 0
          })

          this.classes = response
          this.selectedClassId = response[0].id
          this.getWeeklyAttendances()
          return
        },
        error: (err: any) => {}
      })
    }

    if (this.userRole === UserRoleEnum.Parent || this.userRole === UserRoleEnum.Pupil) this.getWeeklyAttendances()
  }

  public getWeeklyAttendances(): void {
    this.attendanceService.selectWeeklyAttendances(this.date, this.selectedClassId).subscribe({
      next: (response: WeeklyAttendanceDetailsToSelectDTO) => {
        //console.log(JSON.stringify(response, null, 2))
        //console.log(response.pupilsWeeklyAttendances[2].dailyAttendances["Thursday"][1])

        this.workdaysDetails = response.workdays
        this.teachingHoursIds = response.teachingHoursIds
        this.pupilsWeeklyAttendancesDetails = response.pupilsWeeklyAttendances
        this.isAttendanceInitiallyLoading = false
      },
      error: (errr: any) => {}
    })
  }

  public incrementDate(): void {
    this.date.setDate(this.date.getDate() + 7)
    this.getWeeklyAttendances()
  }

  public decrementDate(): void {
    this.date.setDate(this.date.getDate() - 7)
    this.getWeeklyAttendances()
  }

  public displayAttendance(classId: number): void {
    this.selectedClassId = classId
    this.getWeeklyAttendances()
  }

  public getAttendanceTypeName(attendanceType: AttendanceTypeEnum): string {
    switch (attendanceType) {
      case AttendanceTypeEnum.Present: return 'Obecność'
      case AttendanceTypeEnum.Absent: return 'Nieobecność'
      case AttendanceTypeEnum.Late: return 'Spóźnienie'
      case AttendanceTypeEnum.Excused: return 'Zwolnienie'
      case AttendanceTypeEnum.ExcusedAbsence: return 'Nieobecność usprawiedliwiona'
      default: return ''
    }
  }

  public getAttendanceTypeSymbol(attendanceType: AttendanceTypeEnum): string {
    switch (attendanceType) {
      case AttendanceTypeEnum.Present: return 'ob'
      case AttendanceTypeEnum.Absent: return 'nb'
      case AttendanceTypeEnum.Late: return 'sp'
      case AttendanceTypeEnum.Excused: return 'zw'
      case AttendanceTypeEnum.ExcusedAbsence: return 'u'
      default: return ''
    }
  }

  public getAttendanceColor(attendanceType: AttendanceTypeEnum): { [key: string]: string } | null {
    switch (attendanceType) {
      case AttendanceTypeEnum.Present: return { 'background-color': 'rgb(144, 226, 86)' }
      case AttendanceTypeEnum.Absent: return { 'background-color': 'rgb(255, 82, 70)' }
      case AttendanceTypeEnum.Late: return { 'background-color': 'rgb(250, 153, 56)' }
      case AttendanceTypeEnum.Excused: return { 'background-color': 'rgb(171, 171, 171)' }
      case AttendanceTypeEnum.ExcusedAbsence: return { 'background-color': 'rgb(248, 238, 97)' }
      default: return null
    }
  }

  public addAttendance(date: Date, teachingHourId: number, pupilId: number): void {
    if (this.userRole === UserRoleEnum.Admin) {
      this.subjectService.selectSubjects().subscribe({
        next: (response: SubjectDetailsToSelectDTO[]) => {
          let dialogRef = this.matDialog.open(AttendanceDialogComponent, {
            data: {
              title: 'Formularz wystawiania frekwencji',
              subjects: response,
            },
            autoFocus: false
          })
      
          dialogRef.afterClosed().subscribe({
            next: (response?: {
              subject: SubjectDetailsToSelectDTO,
              type: AttendanceTypeEnum
            }) => {
              if (response === undefined) return

              this.attendanceService.insertAttendance({
                date: date,
                teachingHourId: teachingHourId,
                subjectId: response.subject.id,
                pupilId: pupilId,
                issuerId: this.authService.getUserId(),
                type: response.type
              }).subscribe({
                next: (response: number) => {
                  this.getWeeklyAttendances()
                },
                error: (err: any) => {}
              })
            },
            error: (err: any) => {}
          })
        },
        error: (err: any) => {}
      })

    } else if (this.userRole === UserRoleEnum.Teacher) {
      this.subjectService.selectTaughtSubjects(this.selectedClassId!).subscribe({
        next: (response: SubjectDetailsToSelectDTO[]) => {
          let dialogRef = this.matDialog.open(AttendanceDialogComponent, {
            data: {
              title: 'Formularz wystawiania frekwencji',
              subjects: response,
            }, 
            autoFocus: false
          })
      
          dialogRef.afterClosed().subscribe({
            next: (response?: {
              subject: SubjectDetailsToSelectDTO,
              type: AttendanceTypeEnum
            }) => {
              if (response === undefined) return

              this.attendanceService.insertAttendance({
                date: date,
                teachingHourId: teachingHourId,
                subjectId: response.subject.id,
                pupilId: pupilId,
                issuerId: this.authService.getUserId(),
                type: response.type
              }).subscribe({
                next: (response: number) => {
                  this.getWeeklyAttendances()
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
  }

  public editOrDeleteAttendance(attendanceId: number, selectedSubjectId: number, selectedType: AttendanceTypeEnum): void {
    if (this.userRole === UserRoleEnum.Parent || this.userRole === UserRoleEnum.Pupil) return
    
    if (this.userRole === UserRoleEnum.Admin) {
      this.subjectService.selectSubjects().subscribe({
        next: (response: SubjectDetailsToSelectDTO[]) => {
          let dialogRef = this.matDialog.open(AttendanceDialogComponent, {
            data: {
              title: 'Edycja frekwencji',
              subjects: response,
              selectedSubjectId: selectedSubjectId,
              selectedType: selectedType
            },
            autoFocus: false
          })
      
          dialogRef.afterClosed().subscribe({
            next: (response?: {
              subject: SubjectDetailsToSelectDTO,
              type: AttendanceTypeEnum
            } | null) => {
              if (response === undefined) return

              if (response === null) {
                this.attendanceService.deleteAttendance(attendanceId).subscribe({
                  next: (response: any) => {
                    this.getWeeklyAttendances()
                  },
                  error: (err: any) => {}
                })
                return
              }

              if (selectedSubjectId === response.subject.id && selectedType === response.type) return

              this.attendanceService.updateAttendance({
                id: attendanceId,
                subjectId: response.subject.id,
                type: response.type
              }).subscribe({
                next: (response: any) => {
                  this.getWeeklyAttendances()
                },
                error: (err: any) => {}
              })
            },
            error: (err: any) => {}
          })
        },
        error: (err: any) => {}
      })

    } else if (this.userRole === UserRoleEnum.Teacher) {
      this.subjectService.selectTaughtSubjects(this.selectedClassId!).subscribe({
        next: (response: SubjectDetailsToSelectDTO[]) => {
          let dialogRef = this.matDialog.open(AttendanceDialogComponent, {
            data: {
              title: 'Edycja frekwencji',
              subjects: response,
              selectedSubjectId: selectedSubjectId,
              selectedType: selectedType
            }, 
            autoFocus: false
          })
      
          dialogRef.afterClosed().subscribe({
            next: (response?: {
              subject: SubjectDetailsToSelectDTO,
              type: AttendanceTypeEnum
            } | null ) => {
              if (response === undefined) return

              if (response === null) {
                this.attendanceService.deleteAttendance(attendanceId).subscribe({
                  next: (response: any) => {
                    this.getWeeklyAttendances()
                  },
                  error: (err: any) => {}
                })
                return
              }

              if (selectedSubjectId === response.subject.id && selectedType === response.type) return

              this.attendanceService.updateAttendance({
                id: attendanceId,
                subjectId: response.subject.id,
                type: response.type
              }).subscribe({
                next: (response: any) => {
                  this.getWeeklyAttendances()
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
  }

  public showChart(pupilId: number): void {
    this.attendanceService.selectAttendanceStatisticalData(pupilId).subscribe({
      next: (response: AttendanceStatisticalDataToSelectDTO[]) => {
        let dialogRef = this.matDialog.open(AttendanceChartDialogComponent, {
          data: {
            attendanceStatisticalData: response
          },
          autoFocus: false,
          width: '50vw'
        })
      },
      error: (err: any) => {}
    })
  }
}