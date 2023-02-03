import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'
import { tap } from 'rxjs/operators'

import { environment } from 'src/environments/environment'
import { PagedResponse } from '../models/paged-response'
import { PostDetailsToSelectDTO } from './../models/post-details-to-select'
import { PostDetailsToUpdateDTO } from './../models/post-details-to-update'
import { PostDetailsToInsertDTO } from './../models/post-details-to-insert'
import { PostReactionEnum } from './../models/post-reaction.enum'
import { PostReactionDetailsToInsertDTO } from './../models/post-reaction-details-to-insert'
import { PostReactionDetailsToUpdateDTO } from './../models/post-reaction-details-to-update'
import { UserRoleEnum } from './../models/user-role.enum'

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private httpService: HttpClient) { }

  public selectPosts(pageNumber: number, pageSize: number): Observable<PagedResponse<PostDetailsToSelectDTO>> {
    return this.httpService.get<PagedResponse<PostDetailsToSelectDTO>>(environment.apiURL + `/Post?PageNumber=${pageNumber}&PageSize=${pageSize}&SortBy=CreationDate&Order=Descending`, { 
      responseType: 'json' 
    }).pipe(
      tap((response: PagedResponse<PostDetailsToSelectDTO>) => {
        response.payload.forEach((p: PostDetailsToSelectDTO) => {
          if (p.currentUserReaction !== undefined) p.currentUserReaction = PostReactionEnum[p.currentUserReaction.toString() as keyof typeof PostReactionEnum]
          p.authorizedRoles = p.authorizedRoles.map(role => UserRoleEnum[role.toString() as keyof typeof UserRoleEnum])
        })
      })
    )
  }

  public insertPost(postDetailsToInsertDTO: PostDetailsToInsertDTO): Observable<number> {
    return this.httpService.post<number>(environment.apiURL + '/Post', postDetailsToInsertDTO)
  }

  public updatePost(postDetailsToUpdateDTO: PostDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + '/Post', postDetailsToUpdateDTO)
  }

  public deletePost(postId: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Post?postId=${postId}`)
  }

  public insertPostReaction(postReactionDetailsToInsertDTO: PostReactionDetailsToInsertDTO): Observable<any> {
    return this.httpService.post(environment.apiURL + `/Post/Reaction`, postReactionDetailsToInsertDTO)
  }

  public updatePostReaction(postReactionDetailsToUpdateDTO: PostReactionDetailsToUpdateDTO): Observable<any> {
    return this.httpService.patch(environment.apiURL + `/Post/Reaction`, postReactionDetailsToUpdateDTO)
  }

  public deletePostReaction(postId: number): Observable<any> {
    return this.httpService.delete(environment.apiURL + `/Post/Reaction?postId=${postId}`)
  }
}
