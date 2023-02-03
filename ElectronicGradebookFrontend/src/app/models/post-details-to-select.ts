import { UserDetailsToSelectDTO } from './user-details-to-select'
import { UserRoleEnum } from './user-role.enum'
import { PostReactionEnum } from './post-reaction.enum'

export interface PostDetailsToSelectDTO {
    id: number,
    contents: string,
    creationDate: Date,
    author: UserDetailsToSelectDTO,
    authorizedRoles: UserRoleEnum[],
    usersReactions: { [postReaction in keyof typeof PostReactionEnum]: number },
    currentUserReaction?: PostReactionEnum
}