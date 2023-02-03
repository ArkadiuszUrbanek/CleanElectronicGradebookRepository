import { SubjectDetailsToSelectDTO } from './subject-details-to-select'
import { UserDetailsToSelectDTO } from './user-details-to-select'
import { ClassroomDetailsToSelectDTO } from './classroom-details-to-select'
import { WorkdayEnum } from './workday.enum'
import { LessonStatusEnum } from './lesson-status.enum'

export interface LessonDetailsToSelectDTO {
    id: number,
    teacher: UserDetailsToSelectDTO,
    substituteTeacher?: UserDetailsToSelectDTO,
    subject: SubjectDetailsToSelectDTO,
    teachingHourId: number,
    classroom: ClassroomDetailsToSelectDTO,
    workday: WorkdayEnum,
    status: LessonStatusEnum
}