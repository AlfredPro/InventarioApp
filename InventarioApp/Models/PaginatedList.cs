using Microsoft.EntityFrameworkCore;

namespace InventarioApp.Models
{
    public class PaginatedList<T> : List<T>, IPaginatedEntity
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSpace { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, int pageSpace = 3)
        {
            if(pageIndex < 1) pageIndex = 1;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex > TotalPages ? TotalPages : pageIndex;
            PageSpace = pageSpace;
            StartPage = PageIndex - PageSpace;
            EndPage = PageIndex + PageSpace;
            if (StartPage < 1) StartPage = 1;
            if (EndPage > TotalPages) EndPage = TotalPages;
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, int pageSpace = 3)
        {
            var count = await source.CountAsync();
            var totalpages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > totalpages) pageIndex = totalpages;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize, pageSpace);
        }
    }
}
