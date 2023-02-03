using ElectronicGradebook.DTOs.Enums;

namespace ElectronicGradebook.DTOs
{
    public abstract class BasePagedResponse<T, D, E> 
        where T : class 
        where D : class
        where E : Enum
    {
        public int CurrentPageNumber { get; set; }
        public int CurrentPageSize { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfResults { get; set; }
        public int NumberOfResultsOnCurrentPage { get; set; }
        public List<T> Payload { get; set; }

        protected BasePagedResponse(IQueryable<D> source, int pageNumber, int pageSize, E orderBy, EOrder order, int? userId = null)
        {
            int count = source.Count();
            List<T> items = PerformMapping
                (
                    PerformSortingLogic(source, orderBy, order)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                , userId)
                .ToList();

            CurrentPageNumber = pageNumber;
            CurrentPageSize = pageSize;
            TotalNumberOfPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalNumberOfResults = count;
            NumberOfResultsOnCurrentPage = items.Count;
            Payload = new List<T>(items);
        }

        protected abstract IQueryable<D> PerformSortingLogic(IQueryable<D> source, E orderBy, EOrder order);

        protected abstract IQueryable<T> PerformMapping(IQueryable<D> source, int? userId);
    }
}
