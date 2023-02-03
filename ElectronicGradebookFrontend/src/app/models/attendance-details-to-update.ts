import { AttendanceTypeEnum } from './attendance-type.enum'

export interface AttendanceDetailsToUpdateDTO {
    id: number,
    subjectId: number,
    type: AttendanceTypeEnum
}