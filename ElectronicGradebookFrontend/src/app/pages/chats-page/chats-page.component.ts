import { Component, OnInit, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core'

import { Subject, debounceTime, distinctUntilChanged } from 'rxjs'

import { UserService } from './../../services/user.service'
import { MessageService } from './../../services/message.service'
import { AuthService } from 'src/app/services/auth.service'
import { UserDetailsToSelectDTO } from './../../models/user-details-to-select'
import { MessageDetailsToSelectDTO } from './../../models/message-details-to-select'
import { PagedResponse } from './../../models/paged-response'
import { UserRoleEnum } from './../../models/user-role.enum'
import { UserGenderEnum } from './../../models/user-gender.enum'
import { UserSortablePropertiesEnum } from 'src/app/models/user-sortable-properties.enum'
import { OrderEnum } from 'src/app/models/order.enum'

@Component({
  selector: 'app-chat-pages',
  templateUrl: './chats-page.component.html',
  styleUrls: ['./chats-page.component.css']
})
export class ChatsPageComponent implements OnInit {
  @ViewChild('messagesScroll') private messagesScroll!: ElementRef<HTMLDivElement>

  private textAreaRef?: ElementRef<HTMLTextAreaElement>;
  @ViewChild('textArea', { static: false }) set content(content: ElementRef<HTMLTextAreaElement>) {
    if(content) this.textAreaRef = content
  }

  public messageText: string = ''
  public searchPhrase: string = ''
  private modelChangedSubject: Subject<string> = new Subject<string>()
 
  public isUsersListInitiallyLoading: boolean = true
  private isUsersListScrollBlocked: boolean = true

  private usersCurrentPageNumber: number = 1
  private usersTotalNumberOfPages: number = 0

  private shouldScrollMessagesToTheBottom: boolean = false
  private isMessagesScrollBlocked: boolean = true

  private messagesCurrentPageNumber: number = 1
  private messagesTotalNumberOfPages: number = 0

  public users: UserDetailsToSelectDTO[] = []
  public selectedUser?: UserDetailsToSelectDTO
  public userId: number
  public UserRoleEnum: typeof UserRoleEnum = UserRoleEnum
  public UserGenderEnum: typeof UserGenderEnum = UserGenderEnum

  public messages: MessageDetailsToSelectDTO[] = []

  public isUsersListSpinnerVisible: boolean = true

  constructor(private userService: UserService,
              private messageService: MessageService,
              private authService: AuthService,
              private changeDetectorRef: ChangeDetectorRef) {
    this.userId = this.authService.getUserId()
  }

  public ngOnInit(): void {
    this.getUsers()

    this.modelChangedSubject.pipe(debounceTime(1000), distinctUntilChanged()).subscribe((searchPhrase: string) => { 
      this.searchPhrase = searchPhrase
      this.users = []
      this.usersCurrentPageNumber = 1
      this.isUsersListInitiallyLoading = true
      this.isUsersListSpinnerVisible = true
      this.getUsers()
    })
  }

  public ngAfterViewChecked(): void {        
    if (this.shouldScrollMessagesToTheBottom) {
      this.shouldScrollMessagesToTheBottom = false
      this.scrollMessagesToBottom()
    }
  } 

  private getUsers(): void {
    this.userService.selectUsers(this.usersCurrentPageNumber, 20, UserSortablePropertiesEnum.FullName, OrderEnum.Ascending, this.searchPhrase).subscribe({
      next: (response: PagedResponse<UserDetailsToSelectDTO>) => {
        this.isUsersListSpinnerVisible = false
        this.users = this.users.concat(response.payload)
        this.usersTotalNumberOfPages = response.totalNumberOfPages
        this.isUsersListInitiallyLoading = false
        this.isUsersListScrollBlocked = false
      },
      error: () => {} 
    })
  }

  private getMessages(shouldScrollMessagesToTheBottom: boolean): void {
    this.messageService.selectMessages(this.messagesCurrentPageNumber, 15, [this.userId, this.selectedUser!.id]).subscribe({
      next: (response: PagedResponse<MessageDetailsToSelectDTO>) => {
        this.messages = response.payload.reverse().concat(this.messages)
        this.messagesTotalNumberOfPages = response.totalNumberOfPages
        this.shouldScrollMessagesToTheBottom = shouldScrollMessagesToTheBottom
        this.isMessagesScrollBlocked = false
      },
      error: () => {} 
    })
  }

  public usersListScrolledDown(): void {
    if(!this.isUsersListScrollBlocked && this.usersCurrentPageNumber < this.usersTotalNumberOfPages) {
      this.isUsersListScrollBlocked = true
      this.isUsersListSpinnerVisible = true
      this.usersCurrentPageNumber++
      this.getUsers()
    }
  }

  public seachInputChanged(searchPhrase: string): void {
    this.modelChangedSubject.next(searchPhrase);
  }

  public viewMessages(user: UserDetailsToSelectDTO): void {
    if (this.selectedUser?.id === user.id) return

    this.selectedUser = user
    this.messagesCurrentPageNumber = 1
    this.messagesTotalNumberOfPages = 0
    this.isMessagesScrollBlocked = true
    this.messages = []

    this.changeDetectorRef.detectChanges()
    this.messageText = ''
    this.textAreaRef!.nativeElement.value = ''
    this.resizeTextArea()

    this.getMessages(true)
  }

  public messagesScrolledUp(): void {
    if(!this.isMessagesScrollBlocked && this.messagesCurrentPageNumber < this.messagesTotalNumberOfPages) {
      this.isMessagesScrollBlocked = true
      this.messagesCurrentPageNumber++
      this.getMessages(false)
    }
  }

  private scrollMessagesToBottom(): void {
    try {
      this.messagesScroll.nativeElement.scrollTop = this.messagesScroll.nativeElement.scrollHeight
    } catch(err) { }                 
  }

  public getDateDisplayFormat(message: MessageDetailsToSelectDTO, index: number): number | undefined {
    /*
      0 - longDate  27 września 2022
      1 - custom    27 września
      2 - today     Dzisiaj
    */
   const currentDate = new Date() 
    if (index > 0) {
      if (message.timestamp.getFullYear() !== this.messages[index - 1].timestamp.getFullYear()) {
        if (message.timestamp.getFullYear() !== currentDate.getFullYear()) return 0
        if (message.timestamp.getMonth() === currentDate.getMonth() && message.timestamp.getDate() === currentDate.getDate()) return 2
        else return 1
        
      } else {
        if (message.timestamp.getMonth() === this.messages[index - 1].timestamp.getMonth()) {
          if (message.timestamp.getDate() === this.messages[index - 1].timestamp.getDate()) return undefined
          if (message.timestamp.getFullYear() !== currentDate.getFullYear()) return 0
          if (message.timestamp.getMonth() === currentDate.getMonth() && message.timestamp.getDate() === currentDate.getDate()) return 2
          else return 1

        } else {
          if (message.timestamp.getFullYear() !== currentDate.getFullYear()) return 0
          if (message.timestamp.getMonth() !== currentDate.getMonth()) return 1
          if (message.timestamp.getDate() === currentDate.getDate()) return 2

        }
      }
    } else {
      if (message.timestamp.getFullYear() !== currentDate.getFullYear()) return 0
      if (message.timestamp.getMonth() !== currentDate.getMonth()) return 1
      if (message.timestamp.getDate() === currentDate.getDate()) return 2

    }
    return undefined

  }

  public resizeTextArea(): void {
    this.textAreaRef!.nativeElement.style.height = '0'
    this.textAreaRef!.nativeElement.style.height = `${this.textAreaRef!.nativeElement.scrollHeight}px`
  }

  public sendMessage(): void {
    this.messageService.insertMessage({
      text: this.messageText,
      senderId: this.userId,
      receiverId: this.selectedUser!.id
    }).subscribe({
    next: () => {
      this.messageText = ''
      this.textAreaRef!.nativeElement.value = ''
      this.resizeTextArea()


      this.messagesCurrentPageNumber = 1
      this.messagesTotalNumberOfPages = 0
      this.messages = []
      this.getMessages(true)
    },
    error: () => {}
    })
  }
}