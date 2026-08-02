

public interface ICustomerService
{
    Task<PagedResponse<CustomerResponse>> GetCustomersAsync(GetCustomersQuery query, CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetCustomerByIdAsync(Guid customerId, bool includeUsers = false, bool includeStatistics = false, bool includeMetadata = false, CancellationToken cancellationToken = default);
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request, bool sendInvitation = false, CancellationToken cancellationToken = default);
    Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(Guid customerId, bool force = false, bool anonymizeData = false, CancellationToken cancellationToken = default);
}
