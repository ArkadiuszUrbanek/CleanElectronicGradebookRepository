import { UserRoleEnum } from './user-role.enum'

export interface PostDetailsToInsertDTO {
    contents: string,
    authorId: number,
    authorizedRoles: UserRoleEnum[]
}