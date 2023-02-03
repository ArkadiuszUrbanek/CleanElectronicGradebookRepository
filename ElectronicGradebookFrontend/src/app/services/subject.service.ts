import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'

import { environment } from 'src/environments/environment'
import { SubjectDetailsToSelectDTO } from './../models/subject-details-to-select'

@Injectable({
  providedIn: 'root'
})
export class SubjectService {

  constructor(private httpService: HttpClient) { }

  public selectSubjects(): Observable<SubjectDetailsToSelectDTO[]> {
    return this.httpService.get<SubjectDetailsToSelectDTO[]>(environment.apiURL + `/Subject`, { responseType: 'json'})
  }

  public selectTaughtSubjects(classId: number): Observable<SubjectDetailsToSelectDTO[]> {
    return this.httpService.get<SubjectDetailsToSelectDTO[]>(environment.apiURL + `/Subject/Taught?classId=${classId}`, { responseType: 'json' })
  }
}