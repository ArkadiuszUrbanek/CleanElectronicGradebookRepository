export interface PagedResponse<T> {
    currentPageNumber: number,
    currentPageSize: number,
    totalNumberOfPages: number,
    totalNumberOfResults: number,
    numberOfResultsOnCurrentPage: number,
    payload: T[]
}