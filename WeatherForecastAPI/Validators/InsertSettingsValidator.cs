using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Validators
{
    public class InsertSettingsValidator : AbstractValidator<InsertSettings>
    {
        public InsertSettingsValidator()
        {
            RuleFor(x => x.Units)
                .IsInEnum();
        }
    }
}
