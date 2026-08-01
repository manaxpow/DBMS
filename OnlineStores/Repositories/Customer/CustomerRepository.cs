using OnlineStores.Entities;

namespace OnlineStores.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new List<Customer>();

    public Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(customer);
    }

    public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existing != null)
        {
            _customers.Remove(existing);
            _customers.Add(customer);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existing != null)
        {
            _customers.Remove(existing);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Customer>> GetAllAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var customers = _customers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            customers = customers.Where(c => c.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase) || 
                                             c.Email.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }

        if (query.Status.HasValue)
        {
            customers = customers.Where(c => c.Status == query.Status.Value);
        }

        if (query.MemberType.HasValue)
        {
            customers = customers.Where(c => c.MemberType == query.MemberType.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            customers = customers.Where(c => c.Category != null && c.Category.Equals(query.Category, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(customers);
    }
}
