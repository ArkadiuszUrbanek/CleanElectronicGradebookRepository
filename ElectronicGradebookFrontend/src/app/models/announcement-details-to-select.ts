import { UserRoleEnum } from './user-role.enum'
import { UserDetailsToSelectDTO } from './user-details-to-select'

export interface AnnouncementDetailsToSelectDTO {
    id: number,
    title: string,
    contents: string,
    creationDate: Date,
    author: UserDetailsToSelectDTO,
    authorizedRoles: UserRoleEnum[]
}