public interface IStoreService
{
    Task<StoreResponse?> GetStoreAsync(Guid ownerId, bool includeOwner = false, bool includeSettings = false, CancellationToken cancellationToken = default);
    Task<StoreResponse> UpdateStoreAsync(Guid ownerId, UpdateStoreRequest request, CancellationToken cancellationToken = default);
    Task<StoreResponse> UpdateStoreLogoAsync(Guid ownerId, IFormFile logoFile, bool replaceExisting = false, CancellationToken cancellationToken = default);
}
