import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { registerLocaleData } from '@angular/common'
import localePl from '@angular/common/locales/pl'

registerLocaleData(localePl)

import { ToastrModule } from 'ngx-toastr'
import { NgxRangeModule } from 'ngx-range'
import { InfiniteScrollModule } from 'ngx-infinite-scroll'
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap'
import { MatStepperModule } from '@angular/material/stepper'
import { MatButtonModule } from '@angular/material/button'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatOptionModule } from '@angular/material/core'
import { MatSelectModule} from '@angular/material/select'
import { MatDatepickerModule} from '@angular/material/datepicker'
import { MatNativeDateModule } from '@angular/material/core'
import { MAT_DATE_LOCALE } from '@angular/material/core'
import { MatMenuModule } from '@angular/material/menu'
import { MatRadioModule, MAT_RADIO_DEFAULT_OPTIONS } from '@angular/material/radio'
import { MatCheckboxModule, MAT_CHECKBOX_DEFAULT_OPTIONS } from '@angular/material/checkbox'
import { MatDialogModule } from '@angular/material/dialog'
import { MatSlideToggleModule, MAT_SLIDE_TOGGLE_DEFAULT_OPTIONS } from '@angular/material/slide-toggle'
import { RoundProgressModule } from 'angular-svg-round-progressbar'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatTooltipModule, MAT_TOOLTIP_DEFAULT_OPTIONS } from '@angular/material/tooltip'
import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator'

import { AppComponent } from './app.component'
import { AppRoutingModule } from './app-routing.module'
import { CallbackPipe } from './pipes/callback.pipe'
import { AuthInterceptor } from './interceptors/auth.interceptor'
import { ISO8601DateInterceptor } from './interceptors/iso8601-date.interceptor'
import { NavbarComponent } from './components/navbar/navbar.component'
import { DialogComponent } from './components/dialog/dialog.component'
import { LoginPageComponent } from './pages/login-page/login-page.component'
import { AnnouncementsPageComponent } from './pages/announcements-page/announcements-page.component'
import { AnnouncementDialogComponent } from './pages/announcements-page/components/announcement-dialog/announcement-dialog.component'
import { ChatsPageComponent } from './pages/chats-page/chats-page.component'
import { SurveysPageComponent } from './pages/surveys-page/surveys-page.component'
import { MarksPageComponent } from './pages/marks-page/marks-page.component'
import { AttendancePageComponent } from './pages/attendance-page/attendance-page.component'
import { TimetablePageComponent } from './pages/timetable-page/timetable-page.component'
import { SurveyCreationPageComponent } from './pages/survey-creation-page/survey-creation-page.component'
import { MatIconModule } from '@angular/material/icon'
import { SurveyDialogComponent } from './pages/surveys-page/components/survey-dialog/survey-dialog.component'
import { SurveyResultsPageComponent } from './pages/survey-results-page/survey-results-page.component'
import { ChartComponent } from './pages/survey-results-page/components/chart/chart.component'
import { LessonDialogComponent } from './pages/timetable-page/components/lesson-dialog/lesson-dialog.component'
import { LessonExceptionDialogComponent } from './pages/timetable-page/components/lesson-exception-dialog/lesson-exception-dialog.component'
import { MarkDialogComponent } from './pages/marks-page/components/mark-dialog/mark-dialog.component'
import { AttendanceDialogComponent } from './pages/attendance-page/components/attendance-dialog/attendance-dialog.component'
import { MarkChartDialogComponent } from './pages/marks-page/components/mark-chart-dialog/mark-chart-dialog.component'
import { AttendanceChartDialogComponent } from './pages/attendance-page/components/attendance-chart-dialog/attendance-chart-dialog.component'
import { TeachersPageComponent } from './pages/teachers-page/teachers-page.component'
import { AdministrativeToolsPageComponent } from './pages/administrative-tools-page/administrative-tools-page.component'
import { UserCreateDialogComponent } from './pages/administrative-tools-page/components/user-create-dialog/user-create-dialog.component'
import { PostsPageComponent } from './pages/posts-page/posts-page.component';
import { PostDialogComponent } from './pages/posts-page/components/post-dialog/post-dialog.component'

const polishRangeLabel = (page: number, pageSize: number, length: number) => {
  if (length == 0 || pageSize == 0) {
    return `0 z ${length}`
  }

  length = Math.max(length, 0);

  const startIndex = page * pageSize;

  const endIndex =
    startIndex < length
      ? Math.min(startIndex + pageSize, length)
      : startIndex + pageSize

  return `${startIndex + 1} - ${endIndex} z ${length}`
}

function getPolishPaginatorIntl() {
  const paginatorIntl = new MatPaginatorIntl()

  paginatorIntl.itemsPerPageLabel = 'Wyników na stronę:'
  paginatorIntl.nextPageLabel = 'Następna strona'
  paginatorIntl.previousPageLabel = 'Poprzednia strona'
  paginatorIntl.getRangeLabel = polishRangeLabel

  return paginatorIntl
}

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    ChatsPageComponent,
    AnnouncementsPageComponent,
    NavbarComponent,
    DialogComponent,
    AnnouncementDialogComponent,
    SurveysPageComponent,
    MarksPageComponent,
    AttendancePageComponent,
    TimetablePageComponent,
    SurveyCreationPageComponent,
    SurveyDialogComponent,
    SurveyResultsPageComponent,
    ChartComponent,
    LessonDialogComponent,
    LessonExceptionDialogComponent,
    CallbackPipe,
    MarkDialogComponent,
    AttendanceDialogComponent,
    MarkChartDialogComponent,
    AttendanceChartDialogComponent,
    TeachersPageComponent,
    AdministrativeToolsPageComponent,
    UserCreateDialogComponent,
    PostsPageComponent,
    PostDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
        newestOnTop: true,
        timeOut: 3000,
        positionClass: 'toast-top-left'
      }
    ), 
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    InfiniteScrollModule,
    NgxRangeModule,
    MatStepperModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMenuModule,
    MatRadioModule,
    MatCheckboxModule,
    MatIconModule,
    MatDialogModule,
    MatSlideToggleModule,
    RoundProgressModule,
    NgbPopoverModule,
    MatAutocompleteModule,
    MatTooltipModule,
    MatPaginatorModule
  ],
  providers: [{ 
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true 
    }, {
      provide: HTTP_INTERCEPTORS,
      useClass: ISO8601DateInterceptor,
      multi: true 
    }, { 
      provide: MAT_DATE_LOCALE,
      useValue: 'pl-PL' 
    }, {
      provide: MAT_RADIO_DEFAULT_OPTIONS,
      useValue: { color: 'primary' },
    }, {
      provide: MAT_CHECKBOX_DEFAULT_OPTIONS,
      useValue: { color: 'primary' },
    }, {
      provide: MAT_TOOLTIP_DEFAULT_OPTIONS,
      useValue: { disableTooltipInteractivity: true }
    }, {
      provide: MAT_SLIDE_TOGGLE_DEFAULT_OPTIONS,
      useValue: { color: 'primary' }
    }, {
      provide: MatPaginatorIntl,
      useValue: getPolishPaginatorIntl()
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }