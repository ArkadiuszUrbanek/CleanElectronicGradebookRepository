import { PostReactionEnum } from './post-reaction.enum'

export interface PostReactionDetailsToUpdateDTO {
    id: number,
    type: PostReactionEnum
}