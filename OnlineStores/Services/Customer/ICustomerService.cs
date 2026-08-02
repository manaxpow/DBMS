

public interface ICustomerService
{
    Task<PagedResponse<CustomerResponse>> GetCustomersAsync(GetCustomersQuery query, CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetCustomerByIdAsync(Guid customerId, bool includeUsers = false, bool includeStatistics = false, bool includeMetadata = false, CancellationToken cancellationToken = default);
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request, bool sendInvitation = false, CancellationToken cancellationToken = default);
    Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
    Task DeleteCustomerAsync(Guid customerId, bool force = false, bool anonymizeData = false, CancellationToken cancellationToken = default);

    Task<CustomerSummaryResponse> GetCustomerSummaryAsync(GetCustomerSummaryQuery query, CancellationToken cancellationToken = default);
    Task<string> UploadLogoAsync(Guid customerId, Microsoft.AspNetCore.Http.IFormFile file, bool replaceExisting = false, CancellationToken cancellationToken = default);
    
    Task<PagedResponse<CustomerUserSummaryResponse>> GetCustomerUsersAsync(Guid customerId, GetCustomerUsersQuery query, CancellationToken cancellationToken = default);
    Task<CustomerUserSummaryResponse> CreateCustomerUserAsync(Guid customerId, CreateCustomerUserRequest request, bool sendInvitation = false, CancellationToken cancellationToken = default);
    Task<CustomerUserSummaryResponse?> GetCustomerUserByIdAsync(Guid customerId, Guid userId, CancellationToken cancellationToken = default);
    Task<CustomerUserSummaryResponse> UpdateCustomerUserAsync(Guid customerId, Guid userId, UpdateCustomerUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteCustomerUserAsync(Guid customerId, Guid userId, CancellationToken cancellationToken = default);
}
