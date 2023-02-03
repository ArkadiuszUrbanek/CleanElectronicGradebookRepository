import { UserRoleEnum } from './user-role.enum'

export interface PostDetailsToUpdateDTO {
    id: number,
    contents?: string,
    authorizedRoles?: UserRoleEnum[]
}