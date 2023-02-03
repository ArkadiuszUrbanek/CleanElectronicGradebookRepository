import { MarkCategoryEnum } from './mark-category.enum'
import { MarkTypeEnum } from './mark-type.enum'
import { MarkSemesterEnum } from './mark-semester.enum'

export interface MarkDetailsToInsertDTO {
    value: number,
    weight?: number,
    subjectId: number,
    pupilId: number,
    issuerId: number,
    semester?: MarkSemesterEnum,
    type: MarkTypeEnum,
    category?: MarkCategoryEnum
}