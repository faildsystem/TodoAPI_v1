namespace TodoAPI.Helpers
{
    public class QueryObject
    {

        public string? Title { get; set; } = null;

        public bool? IsCompleted { get; set; } = null;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}