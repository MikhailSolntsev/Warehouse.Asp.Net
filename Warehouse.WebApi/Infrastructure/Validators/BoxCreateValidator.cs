﻿using FluentValidation;
using Warehouse.Web.Dto;

namespace Warehouse.Web.Api.Infrastructure.Validators
{
    public class BoxCreateValidator : AbstractValidator<BoxCreateDto>
    {
        public BoxCreateValidator()
        {
            RuleFor(b => b.Length).NotNull().GreaterThan(0);
            RuleFor(b => b.Width).NotNull().GreaterThan(0);
            RuleFor(b => b.Height).NotNull().GreaterThan(0);
            RuleFor(b => b.Weight).NotNull().GreaterThan(0);
            RuleFor(b => b.ExpirationDate).NotNull().GreaterThan(DateTime.MinValue);
        }
    }
}
