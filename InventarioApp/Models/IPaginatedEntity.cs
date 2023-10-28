namespace InventarioApp.Models
{
    public interface IPaginatedEntity
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int PageSpace { get; }
        public int StartPage { get; }
        public int EndPage { get; }
    }
}
