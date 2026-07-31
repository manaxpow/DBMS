using FluentValidation;

public class GetColumnRequestValidator : AbstractValidator<GetColumnRequest>
{
    public GetColumnRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
