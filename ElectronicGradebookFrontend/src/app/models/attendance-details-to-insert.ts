import { AttendanceTypeEnum } from './attendance-type.enum'

export interface AttendanceDetailsToInsertDTO {
    date: Date,
    teachingHourId: number,
    subjectId: number,
    pupilId: number,
    issuerId: number,
    type: AttendanceTypeEnum
}