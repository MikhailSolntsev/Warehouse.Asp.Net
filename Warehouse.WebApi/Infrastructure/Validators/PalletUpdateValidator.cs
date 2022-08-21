using FluentValidation;
using Warehouse.Web.Dto;

namespace Warehouse.Web.Api.Infrastructure.Validators
{
    public class PalletUpdateValidator : AbstractValidator<PalletUpdateDto>
    {
        public PalletUpdateValidator()
        {
            RuleFor(p => p.Id).NotNull().GreaterThan(0);
            RuleFor(p => p.Length).NotNull().GreaterThan(0);
            RuleFor(p => p.Width).NotNull().GreaterThan(0);
            RuleFor(p => p.Height).NotNull().GreaterThan(0);
        }
    }
}
