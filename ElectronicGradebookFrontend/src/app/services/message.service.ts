import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'

import { environment } from 'src/environments/environment'
import { PagedResponse } from '../models/paged-response'
import { MessageDetailsToSelectDTO } from '../models/message-details-to-select'
import { MessageDetailsToInsertDTO } from '../models/message-details-to-insert'

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpService: HttpClient) { }

  public selectMessages(pageNumber: number, pageSize: number, conversationParticipantsIds: [number, number]): Observable<PagedResponse<MessageDetailsToSelectDTO>> {
    return this.httpService.get<PagedResponse<MessageDetailsToSelectDTO>>(environment.apiURL + `/Message?PageNumber=${pageNumber}&PageSize=${pageSize}&SortBy=Timestamp&Order=Descending&CoversationParticipantsIds=${conversationParticipantsIds[0]}&CoversationParticipantsIds=${conversationParticipantsIds[1]}`, { 
      responseType: 'json' 
    })
  }

  public insertMessage(messageDetailsToInsertDTO: MessageDetailsToInsertDTO): Observable<any> {
    return this.httpService.post<any>(environment.apiURL + '/Message', messageDetailsToInsertDTO)
  }
}
