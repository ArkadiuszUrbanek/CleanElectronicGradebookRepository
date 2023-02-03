import { WorkdayEnum } from './workday.enum'
import { AttendanceDetailsToSelectDTO } from './attendance-details-to-select'
import { UserShrinkedDetailsToSelectDTO } from './user-shrinked-details-to-select'

export interface PupilWeeklyAttendanceDetailsToSelectDTO {
    pupil: UserShrinkedDetailsToSelectDTO,
    dailyAttendances: { [workday in keyof typeof WorkdayEnum]: { [teachingHourId: number]: AttendanceDetailsToSelectDTO } }
}