public sealed class CustomerMember
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public CustomerMemberRole Role { get; private set; }
    public CustomerMemberStatus Status { get; private set; }

    public DateTime JoinedAt { get; private set; }
    public DateTime? LastActiveAt { get; private set; }

    private CustomerMember() { }

    public static CustomerMember Create(Guid customerId, Guid userId, CustomerMemberRole role, CustomerMemberStatus status)
    {
        return new CustomerMember
        {
            CustomerId = customerId,
            UserId = userId,
            Role = role,
            Status = status,
            JoinedAt = DateTime.UtcNow
        };
    }

    public void Update(CustomerMemberRole role, CustomerMemberStatus status)
    {
        Role = role;
        Status = status;
    }

    public void UpdateLastActiveAt()
    {
        LastActiveAt = DateTime.UtcNow;
    }
}
