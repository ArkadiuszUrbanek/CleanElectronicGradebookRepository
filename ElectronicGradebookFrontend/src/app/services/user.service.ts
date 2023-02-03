import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable, tap } from 'rxjs'

import { environment } from 'src/environments/environment'
import { PagedResponse } from './../models/paged-response'
import { UserDetailsToSelectDTO } from './../models/user-details-to-select'
import { UserShrinkedDetailsToSelectDTO } from './../models/user-shrinked-details-to-select'
import { UserRoleEnum } from '../models/user-role.enum'
import { UserGenderEnum } from './../models/user-gender.enum'
import { TeacherDetailsToSelectDTO } from './../models/teacher-details-to-select'
import { UserCreateDTO } from './../models/user-create'
import { TeacherContactDetailsToUpdateDTO } from './../models/teacher-contact-details-to-update'
import { UserSortablePropertiesEnum } from './../models/user-sortable-properties.enum'
import { OrderEnum } from './../models/order.enum'
import { UserAccountActivityToUpdateDTO } from './../models/user-account-activity-to-update'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpService: HttpClient) { }

  public selectUsers(pageNumber: number, pageSize: number, sortBy: UserSortablePropertiesEnum, order: OrderEnum, searchPhrase?: string): Observable<PagedResponse<UserDetailsToSelectDTO>> {
    let url = `${environment.apiURL}/User?PageNumber=${pageNumber}&PageSize=${pageSize}&SortBy=${UserSortablePropertiesEnum[sortBy]}&Order=${OrderEnum[order]}`
    if (searchPhrase) url += `&SearchPhrase=${searchPhrase}`

    return this.httpService.get<PagedResponse<UserDetailsToSelectDTO>>(url, { responseType: 'json' }).pipe(
      tap((response: PagedResponse<UserDetailsToSelectDTO>) => {
        response.payload.forEach((user: UserDetailsToSelectDTO) => {
          user.role = UserRoleEnum[user.role.toString() as keyof typeof UserRoleEnum]
          user.gender = UserGenderEnum[user.gender.toString() as keyof typeof UserGenderEnum]
        })
      })
    )
  }

  public selectAllUsers(userRole: UserRoleEnum): Observable<UserShrinkedDetailsToSelectDTO[]> {
    return this.httpService.get<UserShrinkedDetailsToSelectDTO[]>(environment.apiURL + `/User/All?userRole=${UserRoleEnum[userRole]}`, { responseType: 'json' })
  }

  public selectAllPupils(classId: number): Observable<UserShrinkedDetailsToSelectDTO[]> {
    return this.httpService.get<UserShrinkedDetailsToSelectDTO[]>(environment.apiURL + `/User/Pupils/All?classId=${classId}`, { responseType: 'json' })
  }

  public selectAllChildren(): Observable<UserShrinkedDetailsToSelectDTO[]> {
    return this.httpService.get<UserShrinkedDetailsToSelectDTO[]>(environment.apiURL + '/User/Children/All', { responseType: 'json' })
  }

  public selectAllTeachers(classId: number): Observable<TeacherDetailsToSelectDTO[]> {
    return this.httpService.get<TeacherDetailsToSelectDTO[]>(environment.apiURL + `/User/Teachers/All?classId=${classId}`)
  }

  public updateTeacherContactDetails(teacherContactDetailsToUpdateDTO: TeacherContactDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/User/Teacher/ContactData', teacherContactDetailsToUpdateDTO)
  }

  public insertUser(userCreateDTO: UserCreateDTO): Observable<any> {
    return this.httpService.post(environment.apiURL + '/User/Create', userCreateDTO)
  }

  public updateUserAccountActivityStatus(userAccountActivityToUpdateDTO: UserAccountActivityToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/User/Account/Activity', userAccountActivityToUpdateDTO)
  }
}
