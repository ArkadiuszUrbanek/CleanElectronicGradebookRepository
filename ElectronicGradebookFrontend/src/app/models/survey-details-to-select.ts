import { UserDetailsToSelectDTO } from './user-details-to-select'

export interface SurveyDetailsToSelectDTO {
    id: number,
    name: string, 
    description: string, 
    creationDate: Date,
    expirationDate: Date,
    author: UserDetailsToSelectDTO
}