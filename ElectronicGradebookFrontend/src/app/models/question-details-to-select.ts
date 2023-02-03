import { AnswerDetailsToSelectDTO } from './answer-details-to-select'
import { QuestionTypeEnum } from './question-type.enum'

export interface QuestionDetailsToSelectDTO {
	id: number,
	number: number,
	contents: string,
	type: QuestionTypeEnum,
    answers: AnswerDetailsToSelectDTO[]
}