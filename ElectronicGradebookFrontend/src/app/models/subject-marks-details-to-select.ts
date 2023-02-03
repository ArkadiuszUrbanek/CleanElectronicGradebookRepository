import { MarkDetailsToSelectDTO } from './marks-details-to-select'
import { SubjectDetailsToSelectDTO } from './subject-details-to-select'

export interface SubjectMarksDetailsToSelectDTO {
    subject: SubjectDetailsToSelectDTO,
    marks: MarkDetailsToSelectDTO[]
}