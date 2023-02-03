import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'

import { LoginPageComponent } from './pages/login-page/login-page.component'
import { ChatsPageComponent } from './pages/chats-page/chats-page.component'
import { AnnouncementsPageComponent } from './pages/announcements-page/announcements-page.component'
import { SurveysPageComponent } from './pages/surveys-page/surveys-page.component'
import { SurveyCreationPageComponent } from './pages/survey-creation-page/survey-creation-page.component'
import { MarksPageComponent } from './pages/marks-page/marks-page.component'
import { TimetablePageComponent } from './pages/timetable-page/timetable-page.component'
import { AttendancePageComponent } from './pages/attendance-page/attendance-page.component'
import { AuthGuard } from './guards/auth.guard'
import { UserRoleEnum } from './models/user-role.enum'
import { SurveyResultsPageComponent } from './pages/survey-results-page/survey-results-page.component'
import { TeachersPageComponent } from './pages/teachers-page/teachers-page.component'
import { AdministrativeToolsPageComponent } from './pages/administrative-tools-page/administrative-tools-page.component'
import { PostsPageComponent } from './pages/posts-page/posts-page.component'

const routes: Routes = [{
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  }, {
    path: 'login',
    component: LoginPageComponent,
    pathMatch: 'prefix'
  }, {
    path: 'chats',
    component: ChatsPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'announcements',
    component: AnnouncementsPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'surveys',
    component: SurveysPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'survey/create',
    component: SurveyCreationPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'survey/:id/results',
    component: SurveyResultsPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'timetable',
    component: TimetablePageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'attendance',
    component: AttendancePageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'marks',
    component: MarksPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'teachers',
    component: TeachersPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }, {
    path: 'administrativeTools',
    component: AdministrativeToolsPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Admin ]
    }
  }, {
    path: 'posts',
    component: PostsPageComponent,
    pathMatch: 'prefix',
    canActivate: [AuthGuard],
    data: {
      roles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
    }
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }