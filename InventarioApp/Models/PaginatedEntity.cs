namespace InventarioApp.Models
{
    public class PaginatedEntity
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSpace { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
