namespace WebAPI.RequestDtos
{
    public sealed record PaginatedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
