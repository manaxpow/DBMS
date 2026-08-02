using FluentValidation;

public class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
{
    public GetCustomersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100.");

        RuleFor(x => x.CreatedTo)
            .GreaterThanOrEqualTo(x => x.CreatedFrom).WithMessage("CreatedTo cannot be earlier than CreatedFrom.")
            .When(x => x.CreatedTo.HasValue && x.CreatedFrom.HasValue);
    }
}
