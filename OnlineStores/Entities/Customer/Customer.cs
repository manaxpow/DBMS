

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public CustomerStatus Status { get; private set; } = CustomerStatus.Active;
    public CustomerMemberType MemberType { get; private set; } = CustomerMemberType.Regular;
    public string? Category { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastActiveAt { get; private set; }

    private Customer() { }

    public static Customer Create(string name, string email, string? phone, string? category, CustomerMemberType memberType = CustomerMemberType.Regular, CustomerStatus status = CustomerStatus.Active)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Phone = phone,
            Category = category,
            MemberType = memberType,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string email, string? phone, string? category, CustomerMemberType memberType, CustomerStatus status)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Category = category;
        MemberType = memberType;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
