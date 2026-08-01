using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class FluentValidationStoreFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationStoreFilter(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var argumentType = argument.GetType();

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argumentType);

            var validator = _serviceProvider
                .GetService(validatorType);

            if (validator is not IValidator fluentValidator)
            {
                continue;
            }

            var validationContext =
                new ValidationContext<object>(argument);

            var validationResult =
                await fluentValidator.ValidateAsync(
                    validationContext,
                    context.HttpContext.RequestAborted);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                context.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(errors)
                    {
                        Title = "Request validation failed.",
                        Status = StatusCodes.Status400BadRequest,
                        Instance = context.HttpContext.Request.Path
                    });

                return;
            }
        }

        await next();
    }
}
