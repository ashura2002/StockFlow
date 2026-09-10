namespace WebAPI.RequestDtos
{
    public sealed record SearchProductByNameRequest(
        string ProductName,
        int Page = 1,
        int PageSize = 10);
}
