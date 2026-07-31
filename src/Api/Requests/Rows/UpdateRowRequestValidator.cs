using FluentValidation;

public class UpdateRowRequestValidator : AbstractValidator<UpdateRowRequest>
{
    public UpdateRowRequestValidator()
    {
        RuleFor(x => x.Values)
            .NotEmpty()
            .WithMessage("Values cannot be empty.");
    }
}
