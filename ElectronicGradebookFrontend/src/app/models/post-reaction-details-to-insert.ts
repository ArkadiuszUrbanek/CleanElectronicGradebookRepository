import { PostReactionEnum } from './post-reaction.enum'

export interface PostReactionDetailsToInsertDTO {
    id: number,
    type: PostReactionEnum
}