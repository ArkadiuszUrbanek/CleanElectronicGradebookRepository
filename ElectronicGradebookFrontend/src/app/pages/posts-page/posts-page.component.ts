import { Component, OnInit } from '@angular/core'

import { MatDialog } from '@angular/material/dialog'

import { PostService } from './../../services/post.service'
import { UserRoleEnum } from './../../models/user-role.enum'
import { AuthService } from './../../services/auth.service'
import { PostReactionEnum } from './../../models/post-reaction.enum'
import { PagedResponse } from './../../models/paged-response'
import { PostDetailsToSelectDTO } from './../../models/post-details-to-select'
import { DialogService } from './../../services/dialog.service'
import { PostDialogComponent } from './components/post-dialog/post-dialog.component'

@Component({
  selector: 'app-posts-page',
  templateUrl: './posts-page.component.html',
  styleUrls: ['./posts-page.component.css']
})
export class PostsPageComponent implements OnInit {
  public userRole: UserRoleEnum
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum

  private isPostScrollBlocked: boolean = true

  private currentPageNumber: number = 1
  private totalNumberOfPages: number = 0

  public posts: PostDetailsToSelectDTO[] = []
  public PostReactionEnum: typeof PostReactionEnum = PostReactionEnum

  constructor(private dialogService: DialogService,
              private postService: PostService,
              private authService: AuthService,
              private matDialog: MatDialog) {
    this.userRole = this.authService.getUserRole()
  }

  public ngOnInit(): void {
    this.getPosts(false)
  }

  private getPosts(shouldPush: boolean): void {
    this.postService.selectPosts(this.currentPageNumber, 10).subscribe({
      next: (response: PagedResponse<PostDetailsToSelectDTO>) => {
        if (shouldPush) this.posts = this.posts.concat(response.payload)
        else this.posts = response.payload
        this.totalNumberOfPages = response.totalNumberOfPages
        this.isPostScrollBlocked = false
      },
      error: (err: any) => {}
    })
  }

  public onScrollDown(): void {
    if(!this.isPostScrollBlocked && this.currentPageNumber < this.totalNumberOfPages) {
      this.isPostScrollBlocked = true
      this.currentPageNumber++
      this.getPosts(true)
    }
  }

  public timeAgo(time: any): any {
    switch (typeof time) {
      case 'number': break
      case 'string':
        time = +new Date(time)
        break
      case 'object':
        if (time.constructor === Date) time = time.getTime()
        break
      default:
        time = +new Date()
    }

    let time_formats = [
      [1, 'sekundę temu'],
      [2, 'sekundy temu'],
      [3, 'sekundy temu'],
      [4, 'sekundy temu'],
      [60, 'sekund', 1],
      [120, '1 minutę temu', '1 minutę od teraz'],
      [180, '2 minuty temu', '2 minuty od teraz'],
      [240, '3 minuty temu', '3 minuty od teraz'],
      [300, '4 minuty temu', '4 minuty od teraz'],
      [3600, 'minut', 60],
      [7200, '1 godzinę temu', '1 godzinę od teraz'],
      [10800, '2 godziny temu', '2 godziny od teraz'],
      [14400, '3 godziny temu', '3 godziny od teraz'],
      [18000, '4 godziny temu', '4 godziny od teraz'],
      [86400, 'godzin', 3600],
      [172800, 'Wczoraj', 'Jutro'],
      [259200, 'Przedwczoraj', 'Pojutrze'],
      [604800, 'dni', 86400],
      [1209600, 'W zeszłym tygodniu', 'W przyszłym tygodniu'],
      [1814400, '2 tygodnie temu', 'Za 2 tygodnie'],
      [2419200, '3 tygodnie temu', 'Za 3 tygodnie'],
      [3024000, '4 tygodnie temu', 'Za 4 tygodnie'],
      [2419200, 'tygodni', 604800],
      [4838400, 'W zeszłym miesiącu', 'W przyszłym miesiącu'],
      [7257600, '2 miesiące temu', 'Za 2 miesiące'],
      [9676800, '3 miesiące temu', 'Za 3 miesiące'],
      [12096000, '4 miesiące temu', 'Za 4 miesiące'],
      [29030400, 'miesięcy', 2419200],
      [58060800, 'W zeszłym roku', 'W przyszłym roku'],
      [87091200, '2 lata temu', 'Za 2 lata'],
      [116121600, '3 lata temu', 'Za 3 lata'],
      [145152000, '4 lata temu', 'Za 4 lata'],
      [2903040000, 'lat', 29030400]
    ]

    var seconds = (+new Date() - time) / 1000, token = 'temu', list_choice = 1
  
    if (seconds == 0) return 'W tym momencie'
    
    if (seconds < 0) {
      seconds = Math.abs(seconds);
      token = 'od teraz'
      list_choice = 2
    }

    let i: number = 0, format
    while (format = time_formats[i++])
      if (seconds < format[0]) {
        if (typeof format[2] == 'string')
          return format[list_choice]
        else
          return Math.floor(seconds / format[2]) + ' ' + format[1] + ' ' + token
      }
    return time
  }
  
  public onDislikeButtonClicked(postId: number, currentUserReaction?: PostReactionEnum): void {
    if (currentUserReaction === undefined) {
      this.postService.insertPostReaction({
        id: postId,
        type: PostReactionEnum.Dislike
      }).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }

    if (currentUserReaction === PostReactionEnum.Dislike) {
      this.postService.deletePostReaction(postId).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }

    if (currentUserReaction === PostReactionEnum.Like) {
      this.postService.updatePostReaction({
        id: postId,
        type: PostReactionEnum.Dislike
      }).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }
  }

  public onLikeButtonClicked(postId: number, currentUserReaction?: PostReactionEnum): void {
    if (currentUserReaction === undefined) {
      this.postService.insertPostReaction({
        id: postId,
        type: PostReactionEnum.Like
      }).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }

    if (currentUserReaction === PostReactionEnum.Like) {
      this.postService.deletePostReaction(postId).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }

    if (currentUserReaction === PostReactionEnum.Dislike) {
      this.postService.updatePostReaction({
        id: postId,
        type: PostReactionEnum.Like
      }).subscribe({
        next: (response: any) => {
          this.currentPageNumber = 1
          this.getPosts(false)
        },
        error: (err: any) => {}
      })
    }
  }

  public onRemoveButtonClicked(postId: number): void {
    this.dialogService.showDialog('Czy na pewno chcesz usunąć wybrany wpis?',
    () => {
      this.postService.deletePost(postId).subscribe(
        {
          next: (response: any) => {
            this.currentPageNumber = 1
            this.getPosts(false)
  
          },
          error: (err: any) => {}
        }
      )
    },
    () => {})
  }

  public onAddButtonClicked(): void {
    let dialogRef = this.matDialog.open(PostDialogComponent, { 
      data: {
        title: 'Formularz tworzenia wpisu',
        post: {
          contents: '',
          authorizedRoles: []
        }
      }, 
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe({
      next: (value?: {
        contents: string,
        authorizedRoles: UserRoleEnum[] 
      }) => {
        if (value === undefined) return

        this.postService.insertPost(
          {
            contents: value.contents,
            authorId: this.authService.getUserId(),
            authorizedRoles: value.authorizedRoles
          }
        ).subscribe({
          next: response => {
            this.currentPageNumber = 1
            this.getPosts(false)

          },
          error: error => {}
        })
      },
      error: (err: any) => {}
    })
  }

  public onEditButtonClicked(post: PostDetailsToSelectDTO): void {
    let dialogRef = this.matDialog.open(PostDialogComponent, { 
      data: {
        title: 'Edycja wpisu',
        post: {
          contents: post.contents,
          authorizedRoles: post.authorizedRoles
        }
      }, 
      autoFocus: false 
    })

    dialogRef.afterClosed().subscribe({
      next: (value?: {
        contents: string,
        authorizedRoles: UserRoleEnum[] 
      }) => {
        if (value === undefined) return

        this.postService.updatePost(
          {
            id: post.id,
            contents: value.contents,
            authorizedRoles: value.authorizedRoles
          }
        ).subscribe({
          next: (response: any) => {
            this.currentPageNumber = 1
            this.getPosts(false)

          },
          error: (err: any) => {}
        })
      },
      error: (err: any) => {}
    })
  }
}
