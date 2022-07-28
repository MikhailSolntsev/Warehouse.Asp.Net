using FluentValidation;
using Warehouse.Web.Models;

namespace Warehouse.Web.Api.Infrastructure.Validators
{
    public class SkipTakeValidator : AbstractValidator<SkipTakeParametres>
    {
        public SkipTakeValidator()
        {
            RuleFor(p => p.Take).NotNull().GreaterThan(0);
        }
    }
}
