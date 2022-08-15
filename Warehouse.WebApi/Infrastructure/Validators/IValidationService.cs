using FluentValidation;
using FluentValidation.Results;

namespace Warehouse.Web.Api.Infrastructure.Validators;

public interface IValidationService
{
    public ValidationResult Validate<TModel>(TModel model);
}
