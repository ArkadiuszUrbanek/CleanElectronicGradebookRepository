import { Component, OnInit } from '@angular/core'
import { FormGroup, FormBuilder, Validators, FormControl, ValidatorFn, AbstractControl } from '@angular/forms'

import { filter } from 'rxjs'

import { MatDialog } from '@angular/material/dialog'

import { UserRoleEnum } from '../../models/user-role.enum'
import { AuthService } from './../../services/auth.service'
import { ClassService } from './../../services/class.service'
import { UserService } from './../../services/user.service'
import { MarkService } from './../../services/mark.service'
import { ClassDetailsToSelectDTO } from './../../models/class-details-to-select'
import { UserShrinkedDetailsToSelectDTO } from './../../models/user-shrinked-details-to-select'
import { MarkTypeEnum } from 'src/app/models/mark-type.enum'
import { MarkDetailsToSelectDTO } from './../../models/marks-details-to-select'
import { SubjectMarksDetailsToSelectDTO } from './../../models/subject-marks-details-to-select'
import { MarkSemesterEnum } from 'src/app/models/mark-semester.enum'
import { MarkCategoryEnum } from './../../models/mark-category.enum'
import { SubjectDetailsToSelectDTO } from './../../models/subject-details-to-select'
import { MarkDialogComponent } from './components/mark-dialog/mark-dialog.component'
import { MarkChartDialogComponent } from './components/mark-chart-dialog/mark-chart-dialog.component'
import { MarksStatisticalDataToSelectDTO } from './../../models/marks-statistical-data-to-select'

@Component({
  selector: 'app-marks-page',
  templateUrl: './marks-page.component.html',
  styleUrls: ['./marks-page.component.css']
})
export class MarksPageComponent implements OnInit {
  public userRole: UserRoleEnum
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum

  public myForm!: FormGroup
  public classes: ClassDetailsToSelectDTO[] = []
  public pupils: UserShrinkedDetailsToSelectDTO[] = []
  public filteredPupils: UserShrinkedDetailsToSelectDTO[] = []

  public ViewEnum: typeof ViewEnum = ViewEnum

  public subjectsMarks: SubjectMarks[] = []

  private lastlySelectedPupilId!: number

  public MarkTypeEnum: typeof MarkTypeEnum = MarkTypeEnum

  private autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return { 'invalidAutocompleteObject': { value: control.value } }
      }
      return null
    }
  }

  constructor(private authService: AuthService,
              private formBuilder: FormBuilder,
              private classService: ClassService,
              private userService: UserService,
              private markService: MarkService,
              private matDialog: MatDialog) {
    this.userRole = this.authService.getUserRole()

    this.myForm = this.formBuilder.group({})

    if (this.userRole === UserRoleEnum.Admin || this.userRole === UserRoleEnum.Teacher) {
      this.myForm.addControl('class', this.formBuilder.control('', Validators.required))
      this.myForm.addControl('pupil', this.formBuilder.control('', this.autocompleteObjectValidator()))

      this.class.valueChanges.subscribe((classId: number) => {
        this.pupil.setValue('')
        this.pupil.markAsUntouched()
        this.getPupils(classId)
      })

      this.pupil.valueChanges.subscribe((value: any) => {
        if (value) {
          if (typeof value === 'string') this.filteredPupils = this.filterPupils(value)
          else this.filteredPupils = this.filterPupils(value.firstName + ' ' + value.lastName)
        }
        else this.filteredPupils = this.pupils
      })
    }

    if (this.userRole === UserRoleEnum.Parent) this.myForm.addControl('child', this.formBuilder.control('', Validators.required))

    this.myForm.addControl('marksMembership', this.formBuilder.control(ViewEnum.FirstSemester))

    if (this.userRole !== UserRoleEnum.Pupil) {
      this.myForm.statusChanges.pipe(
        filter(() => this.myForm.valid)
      ).subscribe(() => this.onFormValid())
    }
  }

  public ngOnInit(): void {
    if (this.userRole === UserRoleEnum.Pupil) {
      this.selectMarks(this.authService.getUserId())
      return
    }

    if (this.userRole === UserRoleEnum.Admin) {
      this.classService.selectClasses().subscribe({
        next: (classes: ClassDetailsToSelectDTO[]) => {
          if (classes.length === 0) return

          classes.sort((a: ClassDetailsToSelectDTO, b: ClassDetailsToSelectDTO) => {
            let nameA: string = a.name.toLowerCase(), nameB: string = b.name.toLowerCase()
            if (nameA < nameB) return -1
            if (nameA > nameB) return 1
            return 0
          })

          this.classes = classes
          this.class.setValue(classes[0].id)
        },
        error: (err: any) => {}
      })
      return
    }

    if (this.userRole === UserRoleEnum.Teacher) {
      this.classService.selectTaughtClasses().subscribe({
        next: (classes: ClassDetailsToSelectDTO[]) => {
          if (classes.length === 0) return
          
          classes.sort((a: ClassDetailsToSelectDTO, b: ClassDetailsToSelectDTO) => {
            let nameA: string = a.name.toLowerCase(), nameB: string = b.name.toLowerCase()
            if (nameA < nameB) return -1
            if (nameA > nameB) return 1
            return 0
          })
          this.classes = classes
          this.class.setValue(classes[0].id)
        },
        error: (err: any) => {}
      })
      return
    }

    if (this.userRole === UserRoleEnum.Parent) {
      this.userService.selectAllChildren().subscribe({
        next: (children: UserShrinkedDetailsToSelectDTO[]) => {
          if (children.length === 0) return

          this.pupils = children
          this.child.setValue(children[0].id)
          this.lastlySelectedPupilId = children[0].id
        },
        error: (err: any) => {}
      })
      return
    }
  }

  private getPupils(classId: number): void {
    this.userService.selectAllPupils(classId).subscribe({
      next: (pupils: UserShrinkedDetailsToSelectDTO[]) => {
        this.pupils = pupils
        this.filteredPupils = pupils
      },
      error: (err: any) => {}
    })
  }

  public get class(): FormControl {
    return this.myForm.get('class')! as FormControl
  }

  public get pupil(): FormControl {
    return this.myForm.get('pupil')! as FormControl
  }

  public get child(): FormControl {
    return this.myForm.get('child')! as FormControl
  }

  public get marksMembership(): FormControl {
    return this.myForm.get('marksMembership')! as FormControl
  }

  public getPupilName(pupil: UserShrinkedDetailsToSelectDTO): string {
    return pupil ? pupil.firstName + ' ' + pupil.lastName : ''
  }

  private filterPupils(name: string): UserShrinkedDetailsToSelectDTO[] {
    const filteredName = name.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase()
    return this.pupils.filter((pupil: UserShrinkedDetailsToSelectDTO) => {
      let pupilFullName = pupil.firstName + ' ' +  pupil.lastName
      return pupilFullName.normalize('NFD').replace(/\p{Diacritic}/gu, "").toLowerCase().startsWith(filteredName)
    })
  }

  private onFormValid(): void {
    let pupilId: number
    
    switch (this.userRole) {
      case UserRoleEnum.Admin:
      case UserRoleEnum.Teacher: {
        if (this.lastlySelectedPupilId !== undefined && this.lastlySelectedPupilId === this.pupil.value.id) return
        this.lastlySelectedPupilId = this.pupil.value.id
        pupilId = (this.pupil.value as UserShrinkedDetailsToSelectDTO).id
        break
      }
  
      case UserRoleEnum.Parent: {
        if (this.lastlySelectedPupilId !== undefined && this.lastlySelectedPupilId === this.child.value) return
        this.lastlySelectedPupilId = this.child.value
        pupilId = Number(this.child.value)
        break
      }
    }

    this.selectMarks(pupilId!)
  }

  private calculateWeightedAverage(markDetails: MarkDetailsToSelectDTO[]): string {
    let sum: number = 0.0
    let weightsSum: number = 0

    markDetails.forEach(mark => {
      sum += mark.value * mark.weight!
      weightsSum += mark.weight!
    })

    return (Math.round((sum / weightsSum) * 100) / 100).toFixed(2)
  }

  private selectMarks(pupilId: number): void {
    this.markService.selectMarks(pupilId).subscribe({
      next: (response: SubjectMarksDetailsToSelectDTO[]) => {
        this.subjectsMarks = []

        response.forEach(smd => {

          let marksFromFirstSemester = smd.marks.filter(mark => mark.semester === MarkSemesterEnum.First)

          let partialMarksFromFirstSemester = marksFromFirstSemester.filter(mark => mark.type === MarkTypeEnum.Partial)

          let averageFromPartialMarksFromFirstSemester: string | undefined = undefined
          if (partialMarksFromFirstSemester.length !== 0)
            averageFromPartialMarksFromFirstSemester = this.calculateWeightedAverage(partialMarksFromFirstSemester)

          let proposedSemestralMarkFromFirstSemester = marksFromFirstSemester.find(m => m.type === MarkTypeEnum.ProposedSemestral)
          let semestralMarkFromFirstSemester = marksFromFirstSemester.find(m => m.type === MarkTypeEnum.Semestral)

          let marksFromSecondSemester = smd.marks.filter(mark => mark.semester === MarkSemesterEnum.Second)

          let partialMarksFromSecondSemester = marksFromSecondSemester.filter(mark => mark.type === MarkTypeEnum.Partial)

          let averageFromPartialMarksFromSecondSemester: string | undefined = undefined
          if (partialMarksFromSecondSemester.length !== 0)
            averageFromPartialMarksFromSecondSemester = this.calculateWeightedAverage(partialMarksFromSecondSemester)

          let proposedSemestralMarkFromSecondSemester = marksFromSecondSemester.find(m => m.type === MarkTypeEnum.ProposedSemestral)
          let semestralMarkFromSecondSemester = marksFromSecondSemester.find(m => m.type === MarkTypeEnum.Semestral)

          let averageFromSemestralMarks: string | undefined = undefined
          if (semestralMarkFromFirstSemester !== undefined && semestralMarkFromSecondSemester !== undefined)
            averageFromSemestralMarks = (Math.round((semestralMarkFromFirstSemester.value + semestralMarkFromSecondSemester.value) / 2 * 100) / 100).toFixed(2)

          let finalMark = smd.marks.find(mark => mark.type === MarkTypeEnum.Final)

          this.subjectsMarks.push({
            subject: smd.subject,
            firstSemesterMarks: {
              partialMarks: partialMarksFromFirstSemester,
              averageFromPartialMarks: averageFromPartialMarksFromFirstSemester,
              proposedSemestralMark: proposedSemestralMarkFromFirstSemester,
              semestralMark: semestralMarkFromFirstSemester
            },
            secondSemesterMarks: {
              partialMarks: partialMarksFromSecondSemester,
              averageFromPartialMarks: averageFromPartialMarksFromSecondSemester,
              proposedSemestralMark: proposedSemestralMarkFromSecondSemester,
              semestralMark: semestralMarkFromSecondSemester
            },
            yearlymarks: {
              averageFromSemestralMarks: averageFromSemestralMarks,
              finalMark: finalMark
            }
          })
        })
      },
      error: (err: any) => {}
    })
  }

  public getPartialMarkColor(markCategory: MarkCategoryEnum): { [key: string]: string } | null {
    switch (markCategory) {
      case MarkCategoryEnum.Quiz: return { 'background-color': '#29AB87' }
      case MarkCategoryEnum.Test: return { 'background-color': '#50C878' }
      case MarkCategoryEnum.Exam: return { 'background-color': '#4CBB17' }
      case MarkCategoryEnum.OralAnswer: return { 'background-color': '#C7EA46' }
      case MarkCategoryEnum.Exercise: return { 'background-color': '#708238' }
      case MarkCategoryEnum.Activity: return { 'background-color': '#D0F0C0' }
      default: return null
    }
  }

  public getMarkSymbol(markValue: number): string {
    switch (markValue) {
      case 1: return '1'
      case 1.5: return '1+'
      case 1.75: return '2-'
      case 2: return '2'
      case 2.5: return '2+'
      case 2.75: return '3-'
      case 3: return '3'
      case 3.5: return '3+'
      case 3.75: return '4-'
      case 4: return '4'
      case 4.5: return '4+'
      case 4.75: return '5-'
      case 5: return '5'
      case 5.5: return '5+'
      case 5.75: return '6-'
      case 6: return '6'
      default: return ''
    }
  }

  public getPartialMarkCategoryName(markCategory: MarkCategoryEnum): string {
    switch (markCategory) {
      case MarkCategoryEnum.Quiz: return 'kartkówka'
      case MarkCategoryEnum.Test: return 'sprawdzian'
      case MarkCategoryEnum.Exam: return 'egzamin'
      case MarkCategoryEnum.OralAnswer: return 'odpowiedź ustna'
      case MarkCategoryEnum.Exercise: return 'ćwiczenie'
      case MarkCategoryEnum.Activity: return 'aktywność'
      default: return ''
    }
  }

  public addMark(subjectId: number, view: ViewEnum, type: MarkTypeEnum): void {
    let title: string = 'Formularz dodawania '

    switch (type) {
      case MarkTypeEnum.Partial: 
        title += 'oceny bieżącej' 
        break

      case MarkTypeEnum.ProposedSemestral:
        title += 'proponowanej oceny śródrocznej'
        break

      case MarkTypeEnum.Semestral:
        title += 'oceny śródrocznej'
        break  

      case MarkTypeEnum.Final:
        title += 'oceny rocznej'
        break
    }

    let dialogRef = this.matDialog.open(MarkDialogComponent, {
      data: {
        type: type,
        title: title
      }, 
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe(
      (response: {
        category?: MarkCategoryEnum,
        value: number,
        weight?: number
      }) => {
        if (response === undefined) return

        this.markService.insertMark({
          value: response.value,
          weight: response.weight,
          subjectId: subjectId,
          pupilId: this.pupil.value.id,
          issuerId: this.authService.getUserId(),
          semester: view === ViewEnum.FirstSemester ? MarkSemesterEnum.First : view === ViewEnum.SecondSemester ? MarkSemesterEnum.Second : undefined,
          type: type,
          category: response.category
        }).subscribe({
          next: (response: number) => {
            this.selectMarks(this.pupil.value.id)
          },
          error: (err: any) => {}
        })
      }
    )
  }

  public editOrDeleteMark(markId: number, type: MarkTypeEnum, value: number, weight?: number, category?: MarkCategoryEnum): void {
    if (this.userRole === UserRoleEnum.Parent || this.userRole === UserRoleEnum.Pupil) return

    let title: string = 'Edycja '

    switch (type) {
      case MarkTypeEnum.Partial: 
        title += 'oceny bieżącej' 
        break

      case MarkTypeEnum.ProposedSemestral:
        title += 'proponowanej oceny śródrocznej'
        break

      case MarkTypeEnum.Semestral:
        title += 'oceny śródrocznej'
        break  

      case MarkTypeEnum.Final:
        title += 'oceny rocznej'
        break
    }

    let dialogRef = this.matDialog.open(MarkDialogComponent, {
      data: {
        type: type,
        title: title,
        value: value,
        weight: weight,
        category: category
      }, 
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe(
      (response?: {
        category?: MarkCategoryEnum,
        value: number,
        weight?: number
      } | null) => {
        if (response === undefined) return

        if (response === null) {
          this.markService.deleteMark(markId).subscribe({
            next: (response: any) => {
              this.selectMarks(this.pupil.value.id)
            },
            error: (err: any) => {}
          })
          return
        }

        if (value === response!.value && weight === response!.weight && category === response!.category) return

        this.markService.updateMark({
          id: markId,
          value: response!.value,
          weight: response!.weight,
          category: response!.category
        }).subscribe({
          next: (response: number) => {
            this.selectMarks(this.pupil.value.id)
          },
          error: (err: any) => {}
        })
        
      }
    )
  }

  public showChart(subjectId: number): void {
    this.markService.selectPartialMarksStatisticalData(this.lastlySelectedPupilId, subjectId).subscribe({
      next: (response: MarksStatisticalDataToSelectDTO[]) => {
        let dialogRef = this.matDialog.open(MarkChartDialogComponent, {
          data: {
            marksStatisticalData: response
          },
          autoFocus: false,
          width: '50vw'
        })
      },
      error: (err: any) => {}
    })
  }
}

enum ViewEnum {
  FirstSemester = 0,
  SecondSemester = 1,
  SchoolYearEnd = 2
}

interface SubjectMarks {
  subject: SubjectDetailsToSelectDTO,
  firstSemesterMarks: {
    partialMarks: MarkDetailsToSelectDTO[],
    averageFromPartialMarks?: string,
    proposedSemestralMark?: MarkDetailsToSelectDTO,
    semestralMark?: MarkDetailsToSelectDTO
  },
  secondSemesterMarks: {
    partialMarks: MarkDetailsToSelectDTO[],
    averageFromPartialMarks?: string,
    proposedSemestralMark?: MarkDetailsToSelectDTO,
    semestralMark?: MarkDetailsToSelectDTO
  },
  yearlymarks: {
    averageFromSemestralMarks?: string,
    finalMark?: MarkDetailsToSelectDTO
  }
}