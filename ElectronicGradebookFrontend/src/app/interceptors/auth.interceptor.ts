import { Injectable } from '@angular/core'
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http'

import { Observable, tap } from 'rxjs'

import { AuthService } from '../services/auth.service'


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, 
            next: HttpHandler): Observable<HttpEvent<any>> {

    let JWT = this.authService.getToken()

    if (JWT) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${JWT}`,
        },
      })
    }

    return next.handle(request)
  }
}