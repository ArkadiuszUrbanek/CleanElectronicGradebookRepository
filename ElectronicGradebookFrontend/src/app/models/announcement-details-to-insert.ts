import { UserRoleEnum } from './user-role.enum'

export interface AnnouncementDetailsToInsertDTO {
    title: string,
    contents: string,
    authorId: number,
    authorizedRoles: UserRoleEnum[]
}