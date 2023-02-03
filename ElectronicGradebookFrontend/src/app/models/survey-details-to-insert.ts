import { QuestionDetailsToInsertDTO } from './question-details-to-insert'
import { UserRoleEnum } from './user-role.enum'

export interface SurveyDetailsToInsertDTO {
    name: string,
    description: string,
    authorId: number,
    targetedRoles: UserRoleEnum[],
    expirationDate: Date,
    questions: QuestionDetailsToInsertDTO[]
}