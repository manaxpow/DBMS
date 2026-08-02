




public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<PagedResponse<CustomerResponse>> GetCustomersAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(query, cancellationToken);
        if (customers is not null)
            return new PagedResponse<CustomerResponse>(customers.Select(MapToCustomerResponse), customers.Count(), query.Page, query.PageSize);
        return new PagedResponse<CustomerResponse>(Array.Empty<CustomerResponse>(), 0, query.Page, query.PageSize);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(Guid customerId, bool includeUsers = false, bool includeStatistics = false, bool includeMetadata = false, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) return null;
        return MapToCustomerResponse(customer);
    }

    public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request, bool sendInvitation = false, CancellationToken cancellationToken = default)
    {
        var customer = Customer.Create(
            name: request.Name,
            email: request.Email,
            phone: request.Phone,
            category: request.Category,
            memberType: request.MemberType
        );

        await _customerRepository.AddAsync(customer, cancellationToken);
        return MapToCustomerResponse(customer);
    }

    public async Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        customer.Update(request.Name, request.Email, request.Phone, request.Category, request.MemberType, request.Status);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        return MapToCustomerResponse(customer);
    }

    public async Task DeleteCustomerAsync(Guid customerId, bool force = false, bool anonymizeData = false, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");
        await _customerRepository.DeleteAsync(customer, cancellationToken);
    }

    private CustomerResponse MapToCustomerResponse(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.Status,
            customer.MemberType,
            customer.Category,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.LastActiveAt
        );
    }
}
