import { LessonDetailsToSelectDTO } from './lesson-details-to-select'
import { TeachingHourDetailsToSelectDTO } from './teaching-hour-details-to-select'
import { WorkdayDetailsToSelectDTO } from './workday-details-to-select'

export interface WeeklyTimetableDetailsToSelectDTO {
    workdays: WorkdayDetailsToSelectDTO[],
    teachingHours: TeachingHourDetailsToSelectDTO[],
    lessons: LessonDetailsToSelectDTO[]
}