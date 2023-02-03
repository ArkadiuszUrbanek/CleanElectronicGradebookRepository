import { AttendanceTypeEnum } from 'src/app/models/attendance-type.enum'

export interface AttendanceStatisticalDataToSelectDTO {
    label: string,
    data: { [type in keyof typeof AttendanceTypeEnum]: number }
}