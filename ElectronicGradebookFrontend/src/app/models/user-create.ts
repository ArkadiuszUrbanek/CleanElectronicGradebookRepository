import { UserGenderEnum } from './user-gender.enum'
import { UserRoleEnum } from './user-role.enum'

export interface UserCreateDTO {
    firstName: string,
    lastName: string,
    role: UserRoleEnum,
    gender: UserGenderEnum,
    email: string,
    password: string,
    secondName?: string,
    birthDate?: Date,
    contactEmail?: string,
    contactNumber?: string
}