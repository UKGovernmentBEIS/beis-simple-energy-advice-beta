using GovUkDesignSystem.Attributes.ValidationAttributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class WallConstructionViewModel : QuestionFlowViewModel
    {
        [GovUkValidateRequired(ErrorMessageResourceName = nameof(ErrorMessages.WallConstructionRequired), ErrorMessageResourceType = typeof(ErrorMessages))]
        public WallConstruction? WallConstruction { get; set; }
        
        public bool? HintSolidWalls { get; set; }

        public string Reference { get; set; }
        public Epc Epc { get; set; }
    }
}