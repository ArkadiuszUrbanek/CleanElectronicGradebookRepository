import { UserRoleEnum } from './user-role.enum'

export interface AnnouncementDetailsToUpdateDTO {
    id: number,
    title?: string,
    contents?: string,
    authorizedRoles?: UserRoleEnum[]
}