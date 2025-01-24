using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.Auth;

public class AuthViewModel
{
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter password")]
    public string Password { get; set; }

    public string ReturnPath { get; set; }
}