import { LessonStatusEnum } from './lesson-status.enum'

export interface LessonExceptionDetailsToInsertDTO {
    date: Date,
    lessonId: number,
    teacherId?: number,
    status: LessonStatusEnum
}