using OnlineStores.Entities;

public class StoreRepository : IStoreRepository
{
    private readonly List<Store> _stores = new List<Store>();

    public Task AddAsync(Store store, CancellationToken cancellationToken = default)
    {
        _stores.Add(store);
        return Task.CompletedTask;
    }

    public Task<Store?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        var store = _stores.FirstOrDefault(s => s.OwnerId == ownerId);

        // Auto-seed a mock store if it doesn't exist for testing in Swagger
        if (store == null)
        {
            store = Store.Create(ownerId, "My Awesome Mock Store", "This store was automatically generated for testing.");
            store.UpdateLogo("https://example.com/default-logo.png");
            _stores.Add(store);
        }

        return Task.FromResult<Store?>(store);
    }

    public Task UpdateAsync(Store store, CancellationToken cancellationToken = default)
    {
        var existingStore = _stores.FirstOrDefault(s => s.Id == store.Id);
        if (existingStore != null)
        {
            _stores.Remove(existingStore);
            _stores.Add(store);
        }
        return Task.CompletedTask;
    }
}
