import { Component, OnInit } from '@angular/core'

import { AuthService } from '../../services/auth.service'
import { UserRoleEnum } from '../../models/user-role.enum'

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public visibleNavbarItems: NavbarItem[] = []

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    let navbarItems: NavbarItem[] = [{
        path: '/teachers',
        iconClass: 'bi-person',
        bottomText: 'Nauczyciele',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/chats',
        iconClass: 'bi-chat',
        bottomText: 'Wiadomości',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/timetable',
        iconClass: 'bi-calendar',
        bottomText: 'Plan lekcji',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/attendance',
        iconClass: 'bi-list-check',
        bottomText: 'Frekwencja',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/marks',
        iconClass: 'bi-journal-bookmark',
        bottomText: 'Oceny',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/announcements',
        iconClass: 'bi-megaphone',
        bottomText: 'Ogłoszenia',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/posts',
        iconClass: 'bi-card-text',
        bottomText: 'Wpisy',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/surveys',
        iconClass: 'bi-clipboard2-data',
        bottomText: 'Ankiety',
        authorizedRoles: [ UserRoleEnum.Pupil, UserRoleEnum.Parent, UserRoleEnum.Teacher, UserRoleEnum.Admin ]
      }, {
        path: '/administrativeTools',
        iconClass: 'bi-wrench',
        bottomText: 'Narzędzia administracyjne',
        authorizedRoles: [ UserRoleEnum.Admin ]
      }
    ]

    this.visibleNavbarItems = navbarItems.filter(navbarItem => navbarItem.authorizedRoles.includes(this.authService.getUserRole())).reverse()
  }

  public logout(): void {
    this.authService.logout()
  }
}

type NavbarItem = {
  path: string,
  iconClass: string,
  bottomText: string,
  authorizedRoles: UserRoleEnum[]
}