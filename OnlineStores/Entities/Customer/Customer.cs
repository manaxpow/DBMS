

public class Customer
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; } = string.Empty;
    public string? Domain { get; private set; }
    public string? LogoUrl { get; private set; }
    public CustomerStatus Status { get; private set; } = CustomerStatus.Prospect;
    public string? Category { get; private set; }
    public string? Description { get; private set; }
    public ICollection<CustomerMember> Members { get; private set; } = new List<CustomerMember>();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastActiveAt { get; private set; }

    private Customer() { }

    public static Customer Create(string companyName, string? domain, string? logoUrl, string? category, string? description, CustomerStatus status = CustomerStatus.Prospect)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            CompanyName = companyName,
            Domain = domain,
            LogoUrl = logoUrl,
            Category = category,
            Description = description,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string companyName, string? domain, string? logoUrl, string? category, string? description, CustomerStatus status)
    {
        CompanyName = companyName;
        Domain = domain;
        LogoUrl = logoUrl;
        Category = category;
        Description = description;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
