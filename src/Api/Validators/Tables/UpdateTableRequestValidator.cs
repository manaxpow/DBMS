using FluentValidation;

public class UpdateTableRequestValidator : AbstractValidator<UpdateTableRequest>
{
    public UpdateTableRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
    }
}
