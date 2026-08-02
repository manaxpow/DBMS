using FluentValidation;

public class UpdateDiscountRequestValidator : AbstractValidator<UpdateDiscountRequest>
{
    public UpdateDiscountRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Discount code is required.")
            .MaximumLength(50).WithMessage("Discount code cannot exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid discount type.");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Discount value must be greater than 0.")
            .Must((request, value) => 
                request.Type != DiscountType.Percentage || value <= 100)
            .WithMessage("Percentage discount cannot exceed 100%.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(x => x.StartsAt).When(x => x.ExpiresAt.HasValue)
            .WithMessage("Expiration date must be after the start date.");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0).When(x => x.UsageLimit.HasValue)
            .WithMessage("Usage limit must be greater than 0.");
            
        RuleFor(x => x.MinimumRequirementValue)
            .GreaterThan(0).When(x => x.MinimumRequirementValue.HasValue)
            .WithMessage("Minimum requirement value must be greater than 0.");
    }
}
