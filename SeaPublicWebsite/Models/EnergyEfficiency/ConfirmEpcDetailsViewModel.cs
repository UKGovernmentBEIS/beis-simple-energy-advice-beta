using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmEpcDetailsViewModel : QuestionFlowViewModel
    {
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Please confirm whether or not this certificate belongs to your address before continuing")]
        public EpcDetailsConfirmed? EpcDetailsConfirmed { get; set; }
    }
}