using FluentValidation;

public class DeleteColumnRequestValidator : AbstractValidator<DeleteColumnRequest>
{
    public DeleteColumnRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
