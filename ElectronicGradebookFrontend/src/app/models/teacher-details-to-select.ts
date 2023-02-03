import { SubjectDetailsToSelectDTO } from './subject-details-to-select'

export interface TeacherDetailsToSelectDTO {
    id: number,
    firstName: string,
    lastName: string,
    contactEmail?: string,
    contactNumber?: string,
    subjectsTaught: SubjectDetailsToSelectDTO[]
}