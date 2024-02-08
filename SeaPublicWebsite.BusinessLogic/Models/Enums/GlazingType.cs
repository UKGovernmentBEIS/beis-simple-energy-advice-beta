using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum GlazingType
    {
        [Display(ResourceType = typeof(Resources.Enum.GlazingType), Description = nameof(Resources.Enum.GlazingType.SingleGlazed))]
        SingleGlazed,
        [Display(ResourceType = typeof(Resources.Enum.GlazingType), Description = nameof(Resources.Enum.GlazingType.Both))]
        Both,
        [Display(ResourceType = typeof(Resources.Enum.GlazingType), Description = nameof(Resources.Enum.GlazingType.DoubleOrTripleGlazed))]
        DoubleOrTripleGlazed,
        [Display(ResourceType = typeof(Resources.Enum.GlazingType), Description = nameof(Resources.Enum.GlazingType.DoNotKnow))]
        DoNotKnow
    }
}
