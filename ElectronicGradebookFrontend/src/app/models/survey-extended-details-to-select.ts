import { SurveyDetailsToSelectDTO } from './survey-details-to-select'
import { QuestionDetailsToSelectDTO } from './question-details-to-select'

export interface SurveyExtendedDetailsToSelectDTO extends SurveyDetailsToSelectDTO {
    questions: QuestionDetailsToSelectDTO[]
}