import { Location } from '@angular/common'
import { Injectable } from '@angular/core'
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router'

import { AuthService } from '../services/auth.service'
import { UserRoleEnum } from './../models/user-role.enum'

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
              private location: Location,
              private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let userRole: UserRoleEnum | undefined = this.authService.getUserRole()

    if (userRole === undefined) this.router.navigate(['login'])

    if (route.data['roles'].includes(userRole)) return true
    else {
      this.location.back()
      return false
    }
  }
}
