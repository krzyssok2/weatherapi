using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Validators
{
    public class AuthAccountValidator : AbstractValidator<AuthAccount>
    {
        public AuthAccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .NotNull();
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull();

        } 
    }
}
