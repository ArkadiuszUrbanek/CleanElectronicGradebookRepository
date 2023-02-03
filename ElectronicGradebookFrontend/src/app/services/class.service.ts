import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'

import { ClassDetailsToSelectDTO } from './../models/class-details-to-select'
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ClassService {

  constructor(private httpService: HttpClient) { }

  public selectClasses(): Observable<ClassDetailsToSelectDTO[]> {
    return this.httpService.get<ClassDetailsToSelectDTO[]>(environment.apiURL + '/Class', { responseType: 'json' })
  }

  public selectTaughtClasses(): Observable<ClassDetailsToSelectDTO[]> {
    return this.httpService.get<ClassDetailsToSelectDTO[]>(environment.apiURL + '/Class/Taught', { responseType: 'json' })
  }
}
