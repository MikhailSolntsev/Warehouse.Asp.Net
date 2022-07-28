using FluentValidation;
using Warehouse.Web.Models;

namespace Warehouse.Web.Api.Infrastructure.Validators
{
    public class PalletCreateValidator : AbstractValidator<PalletCreateDto>
    {
        public PalletCreateValidator()
        {
            RuleFor(p => p.Length).NotNull().GreaterThan(0);
            RuleFor(p => p.Width).NotNull().GreaterThan(0);
            RuleFor(p => p.Height).NotNull().GreaterThan(0);
        }
    }
}
