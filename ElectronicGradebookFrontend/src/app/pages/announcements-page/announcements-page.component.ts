import { Component, OnInit, ElementRef, ViewChildren, QueryList, OnDestroy } from '@angular/core'

import { ToastrService } from 'ngx-toastr'
import { MatDialog } from '@angular/material/dialog'

import { AuthService } from 'src/app/services/auth.service'
import { AnnouncementService } from '../../services/announcement.service'
import { UserRoleEnum } from '../../models/user-role.enum'
import { AnnouncementDetailsToSelectDTO } from '../../models/announcement-details-to-select'
import { PagedResponse } from './../../models/paged-response'
import { DialogService } from 'src/app/services/dialog.service'
import { AnnouncementDialogComponent } from './components/announcement-dialog/announcement-dialog.component'

@Component({
  selector: 'app-announcements-page',
  templateUrl: './announcements-page.component.html',
  styleUrls: ['./announcements-page.component.css']
})
export class AnnouncementsPageComponent implements OnInit, OnDestroy {
  private announcementsRef?: QueryList<ElementRef<HTMLTableElement>>
  @ViewChildren('announcementsTemplateRef') set content(content: QueryList<ElementRef<HTMLTableElement>>) {
    if (content.length !== 0) {
      this.announcementsRef = content
      this.observer.observe(this.announcementsRef.last.nativeElement)
    }
  }

  public userRole: UserRoleEnum
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  
  public announcements: AnnouncementDetailsToSelectDTO[] = []

  private currentPageNumber: number = 1
  private totalNumberOfPages: number = 0

  private observer!: IntersectionObserver

  public constructor(private dialogService: DialogService,
                     private announcementService: AnnouncementService,
                     private toasterService: ToastrService,
                     private authService: AuthService,
                     private matDialog: MatDialog) {
    this.userRole = this.authService.getUserRole()
  }

  public ngOnInit(): void {
    this.getAnnouncements()
    this.configureIntersectionObserver()
  }

  public ngOnDestroy(): void {
    if (this.announcementsRef?.length) this.observer.unobserve(this.announcementsRef!.last.nativeElement)
  }

  private getAnnouncements(shouldPush: boolean = true): void {
    this.announcementService.selectAnnouncements(this.currentPageNumber, 5).subscribe(
      {
        next: (response: PagedResponse<AnnouncementDetailsToSelectDTO>) => {
          if (shouldPush) {
            this.announcements = this.announcements.concat(response.payload)

          } else {
            this.announcements = response.payload

          }

          this.totalNumberOfPages = response.totalNumberOfPages
        },
        error: error => {
          this.toasterService.error('Nie udało się pobrać najnowszych ogłoszeń.', 'Błąd')

        }
      }
    )
  }

  private configureIntersectionObserver(): void {
    this.observer = new IntersectionObserver((entries: IntersectionObserverEntry[]) => 
      {
        if (entries[0].isIntersecting) {
          this.observer.unobserve(this.announcementsRef!.last.nativeElement)

          if (this.currentPageNumber < this.totalNumberOfPages) {
            this.currentPageNumber++
            this.getAnnouncements()
          }
        }
      }, 
      {
        root: null,
        rootMargin: '0px',
        threshold: 0.5
      }
    )
  }

  public showConfirmationModal(id: number): void {
    this.dialogService.showDialog('Czy na pewno chcesz usunąć wybrane ogłoszenie?',
    () => {
      this.announcementService.deleteAnnouncement(id).subscribe(
        {
          next: response => {
            this.observer.unobserve(this.announcementsRef!.last.nativeElement)
            this.currentPageNumber = 1
            this.getAnnouncements(false)
  
          },
          error: error => {
            this.toasterService.error('Nie udało się usunąć wybranego ogłoszenia.', 'Błąd')
          }
        }
      )
    },
    () => {})
  }

  public showAnnouncementEditingModal(announcement: AnnouncementDetailsToSelectDTO): void {
    let dialogRef = this.matDialog.open(AnnouncementDialogComponent, { 
      data: {
        title: 'Edycja ogłoszenia',
        announcement: {
          title: announcement.title,
          contents: announcement.contents,
          authorizedRoles: announcement.authorizedRoles
        }
      }, 
      autoFocus: false 
    })

    dialogRef.afterClosed().subscribe({
      next: (value?: { 
        title: string,
        contents: string,
        authorizedRoles: UserRoleEnum[] 
      }) => {
        if (value === undefined) return

        this.announcementService.updateAnnouncement(
          {
            id: announcement.id,
            title: value.title,
            contents: value.contents,
            authorizedRoles: value.authorizedRoles
          }
        ).subscribe({
          next: response => {
            this.observer.unobserve(this.announcementsRef!.last.nativeElement)
            this.currentPageNumber = 1
            this.getAnnouncements(false)

          },
          error: error => {
            this.toasterService.error('Nie udało się zedytować wybranego ogłoszenia.', 'Błąd')
          }
        })
      },
      error: (err: any) => {}
    })
  }

  public showAnnouncementAddingModal(): void {
    let dialogRef = this.matDialog.open(AnnouncementDialogComponent, { 
      data: {
        title: 'Formularz tworzenia ogłoszenia',
        announcement: {
          title: '',
          contents: '',
          authorizedRoles: []
        }
      }, 
      autoFocus: false 
    })

    dialogRef.afterClosed().subscribe({
      next: (value?: { 
        title: string,
        contents: string,
        authorizedRoles: UserRoleEnum[] 
      }) => {
        if (value === undefined) return

        this.announcementService.insertAnnouncement(
          {
            title: value.title,
            contents: value.contents,
            authorId: this.authService.getUserId(),
            authorizedRoles: value.authorizedRoles
          }
        ).subscribe({
          next: response => {
            if (this.announcementsRef !== undefined) this.observer.unobserve(this.announcementsRef.last.nativeElement)
            this.currentPageNumber = 1
            this.getAnnouncements(false)

          },
          error: error => {
            this.toasterService.error('Nie udało się zedytować wybranego ogłoszenia.', 'Błąd')
          }
        })
      },
      error: (err: any) => {}
    })
  }
}