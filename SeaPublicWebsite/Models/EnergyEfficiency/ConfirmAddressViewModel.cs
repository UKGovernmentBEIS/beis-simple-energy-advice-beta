﻿using System;
using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using Microsoft.AspNetCore.Mvc.Localization;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ConfirmAddressViewModel : QuestionFlowViewModel
    {
        public List<EpcSearchResult> EpcSearchResults { get; set; }
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.AddressRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public string SelectedEpcId { get; set; }
        public string Reference { get; set; }
        public string Postcode { get; set; }
        public string Number { get; set; }
        
        public Dictionary<string, LabelViewModel> EpcOptionsWithUnlistedOption(IHtmlLocalizer<SharedResources> sharedLocalizer)
        {
            Dictionary<string, LabelViewModel> dict = EpcSearchResults.ToDictionary(
                epc => epc.EpcId,
                epc => new LabelViewModel
                {
                    Text = epc.Address1 + (!String.IsNullOrWhiteSpace(epc.Address2) ? ", " + epc.Address2 : "")
                });
            dict.Add("unlisted", new LabelViewModel
            {
                Text = sharedLocalizer["My address is not listed here"].Value,
            });
            return dict;
        }
    }
}