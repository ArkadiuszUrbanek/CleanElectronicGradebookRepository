import { QuestionTypeEnum } from './question-type.enum'
import { AnswerStatisticalDataToSelectDTO } from './answer-statistical-data-to-select'

export interface QuestionStatisticalDataToSelectDTO {
	number: number,
	contents: string,
	type: QuestionTypeEnum,
    answers: AnswerStatisticalDataToSelectDTO[]
}