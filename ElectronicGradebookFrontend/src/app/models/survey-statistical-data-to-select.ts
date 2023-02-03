import { QuestionStatisticalDataToSelectDTO } from './question-statistical-data-to-select'

export interface SurveyStatisticalDataToSelectDTO {
    name: string,
    description: string,
    timesFinished: number,
    timesUnfinished: number,
    questions: QuestionStatisticalDataToSelectDTO[]
}