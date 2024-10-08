﻿using ElectronicGradebook.DTOs.Enums;

namespace ElectronicGradebook.DTOs
{
    public class BasePaginationParameters<T> where T : Enum
    {
        const int MaxPageSize = 50;
        const int MinPageSize = 1;
        const int MinPageNumber = 1;

        private int _pageNumber = 1;

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = (value <= MinPageNumber) ? MinPageNumber : value;
            }
        }

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value >= MaxPageSize) _pageSize = MaxPageSize;
                else if (value <= MinPageSize) _pageSize = MinPageSize;
                else _pageSize = value;
            }
        }

        public T SortBy { get; set; }
        public EOrder Order { get; set; }
    }
}