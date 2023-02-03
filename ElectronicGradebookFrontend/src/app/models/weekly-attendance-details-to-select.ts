import { PupilWeeklyAttendanceDetailsToSelectDTO } from './pupil-weekly-attendance-details-to-select'
import { WorkdayDetailsToSelectDTO } from './workday-details-to-select'

export interface WeeklyAttendanceDetailsToSelectDTO {
    workdays: WorkdayDetailsToSelectDTO[],
    teachingHoursIds: number[],
    pupilsWeeklyAttendances: PupilWeeklyAttendanceDetailsToSelectDTO[]
}