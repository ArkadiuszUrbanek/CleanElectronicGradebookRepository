import { MarkCategoryEnum } from './mark-category.enum'
import { MarkTypeEnum } from './mark-type.enum'
import { UserDetailsToSelectDTO } from './user-details-to-select'
import { MarkSemesterEnum } from './mark-semester.enum'

export interface MarkDetailsToSelectDTO {
    id: number,
    value: number,
    weight?: number,
    issuer: UserDetailsToSelectDTO,
    semester: MarkSemesterEnum,
    issueDate: Date,
    type: MarkTypeEnum,
    category?: MarkCategoryEnum
}