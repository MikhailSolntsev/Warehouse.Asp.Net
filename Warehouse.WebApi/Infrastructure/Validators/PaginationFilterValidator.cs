using FluentValidation;
using Warehouse.Web.Dto;

namespace Warehouse.Web.Api.Infrastructure.Validators
{
    public class PaginationFilterValidator : AbstractValidator<PaginationFilter>
    {
        public PaginationFilterValidator()
        {
            RuleFor(p => p.Take).NotNull().GreaterThan(0);
        }
    }
}
