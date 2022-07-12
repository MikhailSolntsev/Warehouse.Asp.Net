using FluentValidation;
using Warehouse.Web.Models;

namespace Warehouse.Web.Api.Infrastructure
{
    public class BoxValidator : AbstractValidator<BoxDto>
    {
        public BoxValidator()
        {
            RuleFor(b => b.Length).NotNull().GreaterThan(0);
            RuleFor(b => b.Width).NotNull().GreaterThan(0);
            RuleFor(b => b.Height).NotNull().GreaterThan(0);
            RuleFor(b => b.Weight).NotNull().GreaterThan(0);
            RuleFor(b => b.ExpirationDate).NotNull().GreaterThan(DateTime.MinValue);
        }
    }
}
