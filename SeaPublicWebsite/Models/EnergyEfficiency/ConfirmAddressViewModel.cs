using System;
using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : QuestionFlowViewModel
    {
        public List<EpcSearchResult> EpcSearchResults { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select your address")]
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Confirm whether or not this certificate belongs to your address before continuing")]
        public EpcAddressConfirmed? EpcAddressConfirmed { get; set; }

        public Dictionary<string, LabelViewModel> EpcOptionsWithUnlistedOption()
        {
            Dictionary<string, LabelViewModel> dict = EpcSearchResults.ToDictionary(
                epc => epc.EpcId,
                epc => new LabelViewModel
                {
                    Text = epc.Address1 + (!String.IsNullOrWhiteSpace(epc.Address2) ? ", " + epc.Address2 : "")
                });
            dict.Add("unlisted", new LabelViewModel
            {
                Text = "My address is not listed here",
            });
            return dict;
        }
        
    }
}