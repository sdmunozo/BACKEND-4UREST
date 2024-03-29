﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetShip.DTOs.Common
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int maximumRecordsPerPage = 50;

        public int RecordsPerPage 
        {
            get 
            {
                return recordsPerPage;
            }
            set 
            {
                recordsPerPage = ( value > maximumRecordsPerPage) ? maximumRecordsPerPage : value;
            }
        }
    }
}
