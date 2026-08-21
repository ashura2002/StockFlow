namespace Application.Interfaces
{
    public interface IProductReadRepository
    {
        Task<bool> IsProductNameExistAsync(string productName, CancellationToken ct);
    }
}
