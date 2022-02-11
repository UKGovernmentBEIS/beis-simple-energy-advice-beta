using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class TemperatureViewModel : GovUkViewModel
    {
        public string Title = "What temperature do you set your thermostat to?";
        public string Description = "";
        public QuestionTheme Theme = QuestionTheme.YourHome ;

        [GovUkValidateRequired(ErrorMessageIfMissing = "Please let us know the temperature that your thermostat is set to")]
        public int? Temperature { get; set; }
    }
}