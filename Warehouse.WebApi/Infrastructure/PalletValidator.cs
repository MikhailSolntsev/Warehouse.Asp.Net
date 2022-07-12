using FluentValidation;
using Warehouse.Web.Models;

namespace Warehouse.Web.Api.Infrastructure
{
    public class PalletValidator : AbstractValidator<PalletDto>
    {
        public PalletValidator()
        {
            RuleFor(p => p.Length).NotNull().GreaterThan(0);
            RuleFor(p => p.Width).NotNull().GreaterThan(0);
            RuleFor(p => p.Height).NotNull().GreaterThan(0);
        }
    }
}
