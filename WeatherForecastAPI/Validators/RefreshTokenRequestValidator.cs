using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Token)
                .NotEmpty()
                .NotNull();
        } 
    }
}
