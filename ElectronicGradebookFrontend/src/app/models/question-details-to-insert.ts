import { QuestionTypeEnum } from './question-type.enum'

export interface QuestionDetailsToInsertDTO {
	contents: string,
	type: QuestionTypeEnum,
    answersContents: string[]
}