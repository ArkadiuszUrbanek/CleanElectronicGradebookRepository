import { LessonStatusEnum } from './lesson-status.enum'

export interface LessonExceptionDetailsToUpdateDTO {
    date: Date,
    lessonId: number,
    teacherId?: number,
    status: LessonStatusEnum
}