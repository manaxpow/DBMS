using FluentValidation;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company Name is required.")
            .MaximumLength(100).WithMessage("Company Name cannot exceed 100 characters.");
            
        RuleFor(x => x.Domain)
            .MaximumLength(255).WithMessage("Domain cannot exceed 255 characters.");
    }
}
