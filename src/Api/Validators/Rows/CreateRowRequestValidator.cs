using FluentValidation;

public class CreateRowRequestValidator : AbstractValidator<CreateRowRequest>
{
    public CreateRowRequestValidator()
    {
        RuleFor(x => x.Values)
            .NotEmpty()
            .WithMessage("Values cannot be empty.");
    }
}
