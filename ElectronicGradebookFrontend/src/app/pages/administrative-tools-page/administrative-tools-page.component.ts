import { UserCreateDTO } from './../../models/user-create'
import { UserCreateDialogComponent } from './components/user-create-dialog/user-create-dialog.component'
import { HttpErrorResponse } from '@angular/common/http'
import { Component, OnInit } from '@angular/core'

import { MatDialog } from '@angular/material/dialog'

import { UserService } from './../../services/user.service'
import { UserDetailsToSelectDTO } from './../../models/user-details-to-select'
import { PagedResponse } from './../../models/paged-response'
import { PageEvent } from '@angular/material/paginator'
import { UserGenderEnum } from './../../models/user-gender.enum'
import { UserRoleEnum } from './../../models/user-role.enum'
import { UserSortablePropertiesEnum } from 'src/app/models/user-sortable-properties.enum'
import { OrderEnum } from 'src/app/models/order.enum'
import { AuthService } from 'src/app/services/auth.service'

@Component({
  selector: 'app-administrative-tools-page',
  templateUrl: './administrative-tools-page.component.html',
  styleUrls: ['./administrative-tools-page.component.css']
})
export class AdministrativeToolsPageComponent implements OnInit {
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  public UserGenderEnum: typeof UserGenderEnum = UserGenderEnum
  public UserSortablePropertiesEnum: typeof UserSortablePropertiesEnum = UserSortablePropertiesEnum
  public OrderEnum: typeof OrderEnum = OrderEnum

  public sortBy: UserSortablePropertiesEnum = UserSortablePropertiesEnum.Id
  public order: OrderEnum = OrderEnum.Ascending

  public users: UserDetailsToSelectDTO[] = []

  public pageIndex: number = 0
  public pageSize: number = 10
  public totalNumberOfPages: number = 0
  public length: number = 0
  public readonly pageSizeOptions = [10, 25, 50]

  public userId: number

  constructor(private userService: UserService,
              private authService: AuthService,
              private matDialog: MatDialog) {
    this.userId = this.authService.getUserId()
  }

  ngOnInit(): void {
    this.getUsers()
  }

  private getUsers(): void {
    this.userService.selectUsers(this.pageIndex + 1, this.pageSize, this.sortBy, this.order).subscribe({
      next: (response: PagedResponse<UserDetailsToSelectDTO>) => {
        this.users = response.payload
        this.totalNumberOfPages = response.totalNumberOfPages
        this.length = response.totalNumberOfResults
      },
      error: (err: HttpErrorResponse) => {}
    })
  }

  public handlePageEvent(e: PageEvent) {
    this.length = e.length
    this.pageSize = e.pageSize
    this.pageIndex = e.pageIndex
    this.getUsers()
  }

  private switchOrder(): void {
    this.order === OrderEnum.Descending ? this.order = OrderEnum.Ascending : this.order = OrderEnum.Descending
  }

  public sort(sortBy: UserSortablePropertiesEnum): void {
    if(this.sortBy === sortBy) this.switchOrder()
    else {
      this.sortBy = sortBy
      this.order = OrderEnum.Descending
    }
    this.getUsers()
  }

  public switchUserAccountActivity(userId: number, isActive: boolean): void {
    this.userService.updateUserAccountActivityStatus({
      id: userId,
      isActive: !isActive
    }).subscribe({
      next: (response: any) => { this.getUsers() },
      error: (err: HttpErrorResponse) => {}
    })
  }

  public openUserCreateDialog(): void {
    let dialogRef = this.matDialog.open(UserCreateDialogComponent, {
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe({
      next: (response?: UserCreateDTO) => {
        if (response === undefined) return

        this.userService.insertUser(response).subscribe({
          next: (response: any) => {
            this.getUsers()
          },
          error: (err: any) => {}
        })
      },
      error: (err: any) => {}
    })
  }
}
