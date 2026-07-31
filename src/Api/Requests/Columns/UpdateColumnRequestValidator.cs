using FluentValidation;

public class UpdateColumnRequestValidator : AbstractValidator<UpdateColumnRequest>
{
    public UpdateColumnRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DataType)
            .NotEmpty()
            .Must(type => type.Equals("INT", StringComparison.OrdinalIgnoreCase) ||
                          type.Equals("VARCHAR", StringComparison.OrdinalIgnoreCase) ||
                          type.Equals("DATETIME", StringComparison.OrdinalIgnoreCase) ||
                          type.Equals("BOOLEAN", StringComparison.OrdinalIgnoreCase))
            .WithMessage("DataType must be one of: INT, VARCHAR, DATETIME, BOOLEAN.");
    }
}
