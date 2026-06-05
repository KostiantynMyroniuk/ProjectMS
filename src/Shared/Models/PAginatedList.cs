using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public record PaginatedList<T>(ICollection<T> Data, int PageNumber, int PageSize, int TotalCount) where T : class
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
