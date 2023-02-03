import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router'

import { Observable, tap, Subscription, delay, of } from 'rxjs'

import jwt_decode from 'jwt-decode'
import { ToastrService } from 'ngx-toastr'

import { environment } from 'src/environments/environment'
import { UserRoleEnum } from '../models/user-role.enum'
import { UserCredentialsDTO } from './../models/user-credentials'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenSubscription = new Subscription()

  constructor(private httpService: HttpClient,
              private router: Router,
              private toasterService: ToastrService) {}

  public login(userCredentialsDTO: UserCredentialsDTO): Observable<string> {
    return this.httpService.post(environment.apiURL + "/Auth/LogIn", userCredentialsDTO, { 
        headers: { 'Content-Type': 'application/json'},
        responseType: 'text' 
      }
    ).pipe(
      tap(JWT => { 
          localStorage.setItem('token', JWT)

          const expirationTimeInMilisecondsSinceEpoch: number = Number(jwt_decode<{ [key: string]: string }>(JWT, { header: false })['exp']) * 1000
          const milisecondsSinceEpoch: number = new Date().getTime()
          const offset: number = expirationTimeInMilisecondsSinceEpoch - milisecondsSinceEpoch

          this.tokenSubscription.unsubscribe()
          this.tokenSubscription = of(null).pipe(delay(offset)).subscribe(() => {
              this.logout()
              this.toasterService.info('Nastąpiło automatyczne wylogowanie z konta z powodu stracenia ważności przez token.', '')
            }
          )
        }
      )
    )
  }

  public logout(): void {
    this.tokenSubscription.unsubscribe()
    localStorage.removeItem('token')
    this.router.navigate(['/login'])
  }

  public getToken(): string | null {
    return localStorage.getItem('token')
  }

  public getUserId(): number {
    return Number(jwt_decode<{ [key: string]: string }>(localStorage.getItem('token')!, { header: false })['sub'])
  }

  public getUserRole(): UserRoleEnum {
    let role: string = jwt_decode<{ [key: string]: string }>(localStorage.getItem('token')!, { header: false })['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    return UserRoleEnum[role as keyof typeof UserRoleEnum]
  }
}

/*{
  "iat": "1667936593",
  "sub": "1",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Parent",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname": "super",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname": "tata",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "123@gmail.com",
  "exp": 1667936653,
  "iss": "http://localhost:5000",
  "aud": "http://localhost:4200"
}*/