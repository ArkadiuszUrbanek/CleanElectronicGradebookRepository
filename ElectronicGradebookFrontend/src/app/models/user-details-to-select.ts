import { UserRoleEnum } from "./user-role.enum"
import { UserGenderEnum } from "./user-gender.enum"

export interface UserDetailsToSelectDTO {
    id: number,
    firstName: string,
    lastName: string,
    role: UserRoleEnum,
    gender: UserGenderEnum,
    isActive: boolean
}