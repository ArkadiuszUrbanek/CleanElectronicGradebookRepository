import { WorkdayEnum } from './workday.enum'

export interface WorkdayDetailsToSelectDTO {
    workday: WorkdayEnum,
    date: Date
}