import { HttpErrorResponse } from '@angular/common/http'
import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'

import { AngularDeviceInformationService } from 'angular-device-information'
import { ToastrService } from 'ngx-toastr'

import { AuthService } from './../../services/auth.service'

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  public isPasswordVisible: boolean = false  
  
  constructor(private authService: AuthService,
              private router: Router,
              private toasterService: ToastrService,
              private deviceInformationService: AngularDeviceInformationService) { }

  ngOnInit(): void {
  }

  public switchPasswordVisibility(): void {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  public onSubmit(formData: any): void {

    let deviceType: string = ''
    switch (this.deviceInformationService.getDeviceType())
    {
      case 'Mobile': {
        deviceType = 'Smartfon'
        break
      }

      case 'Desktop': {
        deviceType = 'Komputer'
        break
      }

      default: deviceType = ''
    }

    this.authService.login({
      email: formData.email,
      password: formData.password,
      device: deviceType,
      oS: this.deviceInformationService.getDeviceInfo().os,
      webBrowser: this.deviceInformationService.getDeviceInfo().browser
    }).subscribe({
      next: (response: string) => {
        
        this.router.navigate(['announcements'])
        this.toasterService.success('Logowanie na konto powiodło się.', 'Sukces')
      },
      error: (err: HttpErrorResponse) => {
        this.toasterService.error(err.error, 'Błąd')
      }
    }
    )
  }
}
