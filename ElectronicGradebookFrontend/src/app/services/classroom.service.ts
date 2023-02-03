import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'

import { environment } from 'src/environments/environment'
import { ClassroomDetailsToSelectDTO } from './../models/classroom-details-to-select'

@Injectable({
  providedIn: 'root'
})
export class ClassroomService {

  constructor(private httpService: HttpClient) { }

  public selectClassrooms(): Observable<ClassroomDetailsToSelectDTO[]> {
    return this.httpService.get<ClassroomDetailsToSelectDTO[]>(environment.apiURL + `/Classroom`, { responseType: 'json'})
  }
}
