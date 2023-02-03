import { Component, OnInit } from '@angular/core'

import { forkJoin } from 'rxjs'

import { MatDialog } from '@angular/material/dialog'

import { LessonService } from './../../services/lesson.service'
import { ClassService } from './../../services/class.service'
import { ClassDetailsToSelectDTO } from './../../models/class-details-to-select'
import { WeeklyTimetableDetailsToSelectDTO } from './../../models/weekly-timetable-details-to-select'
import { WorkdayDetailsToSelectDTO } from './../../models/workday-details-to-select'
import { LessonDetailsToSelectDTO } from './../../models/lesson-details-to-select'
import { TeachingHourDetailsToSelectDTO } from './../../models/teaching-hour-details-to-select'
import { WorkdayEnum } from './../../models/workday.enum'
import { AuthService } from './../../services/auth.service'
import { UserRoleEnum } from '../../models/user-role.enum'
import { LessonDialogComponent } from './components/lesson-dialog/lesson-dialog.component'
import { UserService } from './../../services/user.service'
import { SubjectService } from './../../services/subject.service'
import { ClassroomService } from './../../services/classroom.service'
import { DialogService } from './../../services/dialog.service'
import { LessonStatusEnum } from './../../models/lesson-status.enum'
import { ClassroomDetailsToSelectDTO } from './../../models/classroom-details-to-select'
import { UserShrinkedDetailsToSelectDTO } from 'src/app/models/user-shrinked-details-to-select'
import { SubjectDetailsToSelectDTO } from 'src/app/models/subject-details-to-select'
import { LessonExceptionDialogComponent } from './components/lesson-exception-dialog/lesson-exception-dialog.component'

@Component({
  selector: 'app-timetable-page',
  templateUrl: './timetable-page.component.html',
  styleUrls: ['./timetable-page.component.css']
})
export class TimetablePageComponent implements OnInit {
  private date: Date = new Date()
  public selectedClassId!: number
  public classes: ClassDetailsToSelectDTO[] = []
  public workdaysDetails!: WorkdayDetailsToSelectDTO[]
  public teachingHoursDetails!: TeachingHourDetailsToSelectDTO[]
  public WorkdayEnum: typeof WorkdayEnum = WorkdayEnum
  public lessonsDetails: (LessonDetailsToSelectDTO | undefined)[][] = []
  public isTimetableInitiallyLoading: boolean = true
  public userRole: UserRoleEnum
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  public LessonStatusEnum: typeof LessonStatusEnum = LessonStatusEnum

  constructor(private classService: ClassService,
              private lessonService: LessonService,
              private authService: AuthService,
              private matDialog: MatDialog,
              private subjectService: SubjectService,
              private userService: UserService,
              private classroomService: ClassroomService,
              private dialogService: DialogService) { 
    this.userRole = this.authService.getUserRole()
  }

  public ngOnInit(): void {
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
        this.getLessonPlan()
      },
      error: (err: any) => {}
    })
  }

  private getLessonPlan(): void {
    this.lessonService.selectLessonPlan(this.date, this.selectedClassId).subscribe({
      next: (response: WeeklyTimetableDetailsToSelectDTO) => {

        this.lessonsDetails = new Array(response.teachingHours.length)
          .fill(undefined)
          .map(() => new Array(response.workdays.length)
            .fill(undefined))

        response.lessons.forEach((l: LessonDetailsToSelectDTO) => this.lessonsDetails[l.teachingHourId - 1][l.workday] = l)
        this.workdaysDetails = response.workdays.sort((a, b) => a.workday - b.workday)
        this.teachingHoursDetails = response.teachingHours.sort((a, b) => a.id - b.id)
        this.isTimetableInitiallyLoading = false
      },
      error: (err: any) => {}
    })
  }

  public incrementDate(): void {
    this.date.setDate(this.date.getDate() + 7)
    this.getLessonPlan()
  }

  public decrementDate(): void {
    this.date.setDate(this.date.getDate() - 7)
    this.getLessonPlan()
  }

  public displayTimetable(classId: number): void {
    this.selectedClassId = classId
    this.getLessonPlan()
  }

  public addLesson(teachingHourId: number, workday: WorkdayEnum): void {
    forkJoin([
      this.subjectService.selectSubjects(),
      this.userService.selectAllUsers(UserRoleEnum.Teacher),
      this.classroomService.selectClassrooms()]).subscribe(results => {
        let dialogRef = this.matDialog.open(LessonDialogComponent, { 
          data: {
            title: 'Kreator lekcji',
            subjects: results[0],
            teachers: results[1],
            classrooms: results[2]
          }, 
          autoFocus: false 
        })

        dialogRef.afterClosed().subscribe({
          next: (value: {
            subject: SubjectDetailsToSelectDTO,
            teacher: UserShrinkedDetailsToSelectDTO,
            classroom: ClassroomDetailsToSelectDTO
          }) => {
            if (value !== undefined) { 
              this.lessonService.insertLesson({
                classId: this.selectedClassId,
                teacherId: value.teacher.id,
                subjectId: value.subject.id,
                teachingHourId: teachingHourId,
                classroomId: value.classroom.id,
                workday: workday
              }).subscribe({
                next: (value: any) => {
                  this.getLessonPlan()
                },
                error: (err: any) => {}
              })
            }
          },
          error: (err: any) => {}
        })
    })
  }

  public editLesson(lessonId: number,
                    currentSubjectId: number,
                    currentTeacherId: number,
                    currentClassroomId: number): void {
    forkJoin([
      this.subjectService.selectSubjects(),
      this.userService.selectAllUsers(UserRoleEnum.Teacher),
      this.classroomService.selectClassrooms()]).subscribe(results => {
        let dialogRef = this.matDialog.open(LessonDialogComponent, { 
          data: {
            title: 'Edytor lekcji',
            subjects: results[0],
            selectedSubjectId: currentSubjectId,
            teachers: results[1],
            selectedTeacherId: currentTeacherId,
            classrooms: results[2],
            selectedClassroomId: currentClassroomId
          }, 
          autoFocus: false 
        })

        dialogRef.afterClosed().subscribe({
          next: (value: {
            subject: SubjectDetailsToSelectDTO,
            teacher: UserShrinkedDetailsToSelectDTO,
            classroom: ClassroomDetailsToSelectDTO
          }) => {
            if (value !== undefined && (value.subject.id !== currentSubjectId || value.teacher.id !== currentTeacherId || value.classroom.id !== currentClassroomId)) {
              this.lessonService.updateLesson({
                id: lessonId,
                teacherId: value.teacher.id,
                subjectId: value.subject.id,
                classroomId: value.classroom.id
              }).subscribe({
                next: (value: any) => {
                  this.getLessonPlan()
                },
                error: (err: any) => {}
              })
            }
          },
          error: (err: any) => {}
        })
    })
  }

  public deleteLesson(lessonId: number): void {
    this.dialogService.showDialog('Czy na pewno chcesz usunąć wybraną lekcję?',
    () => {
      this.lessonService.deleteLesson(lessonId).subscribe({
        next: (response: any) => {
          this.getLessonPlan()
        },
        error: (err: any) => {}
      })
    },
    () => {})
  }

  public modifyLessonStatus(lessonId: number, date: Date, lessonStatus: LessonStatusEnum, substituteTeacherId?: number): void {
    this.userService.selectAllUsers(UserRoleEnum.Teacher).subscribe({
      next: (teachers: UserShrinkedDetailsToSelectDTO[]) => {
        let dialogRef = this.matDialog.open(LessonExceptionDialogComponent, { 
          data: {
            lessonStatus: lessonStatus,
            teachers: teachers,
            substituteTeacherId: substituteTeacherId
          }, 
          autoFocus: false
        })

        dialogRef.afterClosed().subscribe({
          next: (value: {
            lessonStatus: LessonStatusEnum,
            teacherSubstitutor: UserShrinkedDetailsToSelectDTO | undefined
          }) => {
            if (value !== undefined) {
              if (lessonStatus === LessonStatusEnum.AsPlanned && (value.lessonStatus === LessonStatusEnum.Cancelled || value.lessonStatus === LessonStatusEnum.Substitution)) {
                this.lessonService.insertLessonException({
                  date: date,
                  lessonId: lessonId,
                  teacherId: value.teacherSubstitutor?.id,
                  status: value.lessonStatus
                }).subscribe({
                  next: (value: any) => {
                    this.getLessonPlan()
                  },
                  error: (err: any) => {}
                })
              }

              if ((lessonStatus === LessonStatusEnum.Cancelled && value.lessonStatus === LessonStatusEnum.Substitution) ||
                  (lessonStatus === LessonStatusEnum.Substitution && value.lessonStatus === LessonStatusEnum.Cancelled) ||
                  (lessonStatus === LessonStatusEnum.Substitution && value.lessonStatus === LessonStatusEnum.Substitution)) {
                this.lessonService.updateLessonException({
                  date: date,
                  lessonId: lessonId,
                  teacherId: value.teacherSubstitutor?.id,
                  status: value.lessonStatus
                }).subscribe({
                  next: (value: any) => {
                    this.getLessonPlan()
                  },
                  error: (err: any) => {}
                })
              }

              if ((lessonStatus === LessonStatusEnum.Cancelled || lessonStatus === LessonStatusEnum.Substitution) && value.lessonStatus === LessonStatusEnum.AsPlanned) {
                this.lessonService.deleteLessonException(date, lessonId).subscribe({
                  next: (value: any) => {
                    this.getLessonPlan()
                  },
                  error: (err: any) => {}
                })
              }
              
            }
          },
          error: (err: any) => {}
        })
      },
      error: (err: any) => {}
    })
  }
}