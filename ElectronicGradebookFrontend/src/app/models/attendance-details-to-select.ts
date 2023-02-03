import { SubjectDetailsToSelectDTO } from 'src/app/models/subject-details-to-select'
import { AttendanceTypeEnum } from './attendance-type.enum'
import { UserShrinkedDetailsToSelectDTO } from './user-shrinked-details-to-select'

export interface AttendanceDetailsToSelectDTO {
    id: number,
    issueDate: Date,
    subject: SubjectDetailsToSelectDTO,
    issuer: UserShrinkedDetailsToSelectDTO,
    type: AttendanceTypeEnum
}