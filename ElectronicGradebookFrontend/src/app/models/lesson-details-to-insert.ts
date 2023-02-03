import { WorkdayEnum } from './workday.enum'

export interface LessonDetailsToInsertDTO {
    classId: number,
    teacherId: number,
    subjectId: number,
    teachingHourId: number,
    classroomId: number,
    workday: WorkdayEnum
}