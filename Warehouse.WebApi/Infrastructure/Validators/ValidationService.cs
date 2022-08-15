using FluentValidation;
using FluentValidation.Results;

namespace Warehouse.Web.Api.Infrastructure.Validators;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider serviceProvider;
    public ValidationService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public ValidationResult Validate<TModel>(TModel model)
    {
        var validator = serviceProvider.GetRequiredService<IValidator<TModel>>();
        return validator.Validate(model);
    }
}
