import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'
import { tap } from 'rxjs/operators'

import { environment } from 'src/environments/environment'
import { PagedResponse } from '../models/paged-response'
import { AnnouncementDetailsToSelectDTO } from '../models/announcement-details-to-select'
import { AnnouncementDetailsToUpdateDTO } from '../models/announcement-details-to-update'
import { AnnouncementDetailsToInsertDTO } from './../models/announcement-details-to-insert'
import { UserRoleEnum } from './../models/user-role.enum'

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {

  constructor(private httpService: HttpClient) { }

  public selectAnnouncements(pageNumber: number, pageSize: number): Observable<PagedResponse<AnnouncementDetailsToSelectDTO>> {
    return this.httpService.get<PagedResponse<AnnouncementDetailsToSelectDTO>>(environment.apiURL + `/Announcement?PageNumber=${pageNumber}&PageSize=${pageSize}&SortBy=CreationDate&Order=Descending`, { 
      responseType: 'json' 
    }).pipe(
      tap((response: PagedResponse<AnnouncementDetailsToSelectDTO>) => {
        response.payload.forEach((a: AnnouncementDetailsToSelectDTO) => {
          a.authorizedRoles = a.authorizedRoles.map(role => UserRoleEnum[role.toString() as keyof typeof UserRoleEnum])
        })
      })
    )
  }

  public deleteAnnouncement(id: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Announcement?id=${id}`)
  }

  public updateAnnouncement(announcementDetailsToUpdateDTO: AnnouncementDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/Announcement', announcementDetailsToUpdateDTO)
  }

  public insertAnnouncement(announcementDetailsToInsertDTO: AnnouncementDetailsToInsertDTO): Observable<any> {
    return this.httpService.post(environment.apiURL + '/Announcement', announcementDetailsToInsertDTO)
  }
}