

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;

    public StoreService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<StoreResponse?> GetStoreAsync(Guid ownerId, bool includeOwner = false, bool includeSettings = false, CancellationToken cancellationToken = default)
    {
        var store = await _storeRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (store is null)
        {
            return null;
        }
        return MapToStoreResponse(store);
    }

    public async Task<StoreResponse> UpdateStoreAsync(Guid ownerId, UpdateStoreRequest request, CancellationToken cancellationToken = default)
    {
        var store = await _storeRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (store is null)
        {
            throw new NotFoundException("Store not found");
        }

        store.Update(request.Name, request.Description);

        await _storeRepository.UpdateAsync(store, cancellationToken);

        return MapToStoreResponse(store);
    }

    public async Task<StoreResponse> UpdateStoreLogoAsync(Guid ownerId, IFormFile logoFile, bool replaceExisting = false, CancellationToken cancellationToken = default)
    {
        var store = await _storeRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        if (store is null)
        {
            throw new NotFoundException("Store not found");
        }

        store.UpdateLogo($"https://example.com/logos/{logoFile.FileName}");

        await _storeRepository.UpdateAsync(store, cancellationToken);

        return MapToStoreResponse(store);
    }

    private StoreResponse MapToStoreResponse(Store store)
    {
        return new StoreResponse(
            store.Id,
            store.OwnerId,
            store.Name,
            store.Description,
            store.LogoUrl,
            store.CreatedAt,
            store.UpdatedAt);
    }
}
