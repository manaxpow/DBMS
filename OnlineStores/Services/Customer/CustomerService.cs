




public class CustomerService(ICustomerRepository customerRepository, IFileStorageService fileStorageService) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

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
            companyName: request.CompanyName,
            domain: request.Domain,
            logoUrl: null,
            category: request.Category,
            description: request.Description,
            status: request.Status
        );

        await _customerRepository.AddAsync(customer, cancellationToken);
        return MapToCustomerResponse(customer);
    }

    public async Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        customer.Update(request.CompanyName, request.Domain, customer.LogoUrl, request.Category, request.Description, request.Status);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        return MapToCustomerResponse(customer);
    }

    public async Task DeleteCustomerAsync(Guid customerId, bool force = false, bool anonymizeData = false, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");
        await _customerRepository.DeleteAsync(customer, cancellationToken);
    }

    public async Task<CustomerSummaryResponse> GetCustomerSummaryAsync(GetCustomerSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(new GetCustomersQuery(null, null, null, query.From, query.To, null), cancellationToken);
        var totalCustomers = customers.Count();
        var totalMembers = customers.SelectMany(c => c.Members).Count();
        var activeNow = customers.Count(c => c.Status == CustomerStatus.Customer);

        return new CustomerSummaryResponse(totalCustomers, 0m, totalMembers, 0m, activeNow);
    }

    public async Task<string> UploadLogoAsync(Guid customerId, Microsoft.AspNetCore.Http.IFormFile file, bool replaceExisting = false, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var fileUrl = await _fileStorageService.UploadFileAsync(file, "customers/logos", cancellationToken);
        
        customer.Update(customer.CompanyName, customer.Domain, fileUrl, customer.Category, customer.Description, customer.Status);
        await _customerRepository.UpdateAsync(customer, cancellationToken);
        
        return fileUrl;
    }

    public async Task<PagedResponse<CustomerUserSummaryResponse>> GetCustomerUsersAsync(Guid customerId, GetCustomerUsersQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var members = customer.Members.AsEnumerable();
        
        if (query.Status.HasValue) members = members.Where(m => m.Status == query.Status.Value);
        if (query.Role.HasValue) members = members.Where(m => m.Role == query.Role.Value);
        
        var responses = members.Select(m => new CustomerUserSummaryResponse(m.User.Id, m.User.FullName, m.User.AvatarUrl)).ToList();
        return new PagedResponse<CustomerUserSummaryResponse>(responses, responses.Count, query.Page, query.PageSize);
    }

    public async Task<CustomerUserSummaryResponse> CreateCustomerUserAsync(Guid customerId, CreateCustomerUserRequest request, bool sendInvitation = false, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var user = User.Create(request.Email, request.FullName, "placeholder_hash", "User");
        var member = CustomerMember.Create(customerId, user.Id, request.Role, CustomerMemberStatus.Active);
        
        customer.Members.Add(member);
        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return new CustomerUserSummaryResponse(user.Id, user.FullName, user.AvatarUrl);
    }

    public async Task<CustomerUserSummaryResponse?> GetCustomerUserByIdAsync(Guid customerId, Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var member = customer.Members.FirstOrDefault(m => m.UserId == userId);
        if (member is null) return null;

        return new CustomerUserSummaryResponse(member.User.Id, member.User.FullName, member.User.AvatarUrl);
    }

    public async Task<CustomerUserSummaryResponse> UpdateCustomerUserAsync(Guid customerId, Guid userId, UpdateCustomerUserRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var member = customer.Members.FirstOrDefault(m => m.UserId == userId);
        if (member is null) throw new NotFoundException("Customer member not found");

        member.Update(request.Role, request.Status);
        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return new CustomerUserSummaryResponse(member.User.Id, member.User.FullName, member.User.AvatarUrl);
    }

    public async Task DeleteCustomerUserAsync(Guid customerId, Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null) throw new NotFoundException("Customer not found");

        var member = customer.Members.FirstOrDefault(m => m.UserId == userId);
        if (member is null) throw new NotFoundException("Customer member not found");

        customer.Members.Remove(member);
        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }

    private CustomerResponse MapToCustomerResponse(Customer customer)
    {
        var previewUsers = customer.Members
            .Take(5)
            .Select(x => new CustomerUserSummaryResponse(
                x.User.Id,
                x.User.FullName,
                x.User.AvatarUrl))
            .ToList();

        return new CustomerResponse(
            customer.Id,
            customer.CompanyName,
            customer.LogoUrl,
            customer.Domain,
            customer.Status,
            customer.Category,
            customer.Description,
            customer.Members.Count,
            previewUsers,
            customer.CreatedAt,
            customer.LastActiveAt
        );
    }
}
