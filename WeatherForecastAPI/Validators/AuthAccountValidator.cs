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
        enum Errors
        {
            CantBeEmpty=0,
            MustBeEmail=1,
            CantBeNull=2,
        }
        public AuthAccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()                
                .WithMessage(Errors.CantBeEmpty.ToString())
                .EmailAddress()
                .WithMessage(Errors.MustBeEmail.ToString())
                .NotNull()
                .WithMessage(Errors.CantBeNull.ToString());
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(Errors.CantBeEmpty.ToString())
                .NotNull()
                .WithMessage(Errors.CantBeNull.ToString());

        } 
    }
}
