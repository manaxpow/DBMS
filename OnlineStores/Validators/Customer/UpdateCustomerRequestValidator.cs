using FluentValidation;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");
            
        RuleFor(x => x.MemberType)
            .IsInEnum().WithMessage("Invalid member type.");
            
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid customer status.");
    }
}
