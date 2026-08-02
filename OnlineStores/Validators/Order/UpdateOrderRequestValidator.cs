using FluentValidation;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleForEach(x => x.Items).ChildRules(item => 
        {
            item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
            item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        });
    }
}
