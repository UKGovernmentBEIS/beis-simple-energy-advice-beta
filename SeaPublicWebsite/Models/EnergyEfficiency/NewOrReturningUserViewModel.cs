using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class NewOrReturningUserViewModel : QuestionFlowViewModel
    {
        
        [GovUkValidateRequired(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = nameof(ErrorMessages.UsedServiceBeforeRequired))]
        public NewOrReturningUser? NewOrReturningUser { get; set; }

        [GovUkValidateRequiredIf(ErrorMessageResourceName = nameof(ErrorMessages.ReferenceRequired), ErrorMessageResourceType = typeof(ErrorMessages),
            IsRequiredPropertyName = nameof(RefRequired))]
        public string Reference { get; set; }
        public bool RefRequired => NewOrReturningUser == EnergyEfficiency.NewOrReturningUser.ReturningUser;
        
    }
    
    public enum NewOrReturningUser
    {
        [GovUkRadioCheckboxLabelText(Text = "No, this is my first visit or I don’t have a reference code")] //TODO also an enum here to change
        NewUser,
        [GovUkRadioCheckboxLabelText(Text = "Yes, and I have the 8-character reference code from my previous visit")] //TODO also an enum here to change
        ReturningUser,
    }
}