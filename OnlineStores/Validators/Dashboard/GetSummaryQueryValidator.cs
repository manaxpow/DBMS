using FluentValidation;

public class GetSummaryQueryValidator : AbstractValidator<GetSummaryQuery>
{
    public GetSummaryQueryValidator()
    {
        RuleFor(x => x.To)
            .GreaterThanOrEqualTo(x => x.From).When(x => x.From.HasValue && x.To.HasValue)
            .WithMessage("'To' date must be greater than or equal to 'From' date.");

        RuleFor(x => x.Timezone)
            .MaximumLength(50).WithMessage("Timezone cannot exceed 50 characters.");

        RuleFor(x => x.Currency)
            .Length(3).When(x => !string.IsNullOrEmpty(x.Currency))
            .WithMessage("Currency must be exactly 3 characters (e.g., USD, VND).");
    }
}
