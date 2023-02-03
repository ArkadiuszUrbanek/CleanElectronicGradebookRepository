import { MarkCategoryEnum } from './mark-category.enum'

export interface MarkDetailsToUpdateDTO {
    id: number,
    value?: number,
    weight?: number,
    category?: MarkCategoryEnum
}