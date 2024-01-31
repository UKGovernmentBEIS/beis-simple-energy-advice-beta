using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes.ValidationAttributes;
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
        [Display(ResourceType = typeof(Resources.Enum.NewOrReturningUser), Description = nameof(Resources.Enum.NewOrReturningUser.NewUser))] 
        NewUser,
        [Display(ResourceType = typeof(Resources.Enum.NewOrReturningUser), Description = nameof(Resources.Enum.NewOrReturningUser.ReturningUser))] 
        ReturningUser,
    }
}