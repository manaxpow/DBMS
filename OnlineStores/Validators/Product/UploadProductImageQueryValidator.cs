using FluentValidation;

public class UploadProductImageQueryValidator : AbstractValidator<UploadProductImageQuery>
{
    public UploadProductImageQueryValidator()
    {
        RuleFor(x => x.Position)
            .GreaterThanOrEqualTo(0).WithMessage("Position cannot be negative.")
            .When(x => x.Position.HasValue);
    }
}
