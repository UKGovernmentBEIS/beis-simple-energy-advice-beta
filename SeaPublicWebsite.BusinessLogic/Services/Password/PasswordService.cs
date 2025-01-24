using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace SeaPublicWebsite.BusinessLogic.Services.Password;

public class PasswordService(IOptions<PasswordConfiguration> options)
{
    public bool HashMatchesConfiguredPassword(string hash)
    {
        var configuredPasswordHash = GetConfiguredPasswordHash();

        return configuredPasswordHash != null && configuredPasswordHash == hash;
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashValue = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashValue);
    }

    public string GetConfiguredPasswordHash()
    {
        return options.Value.Password != null ? HashPassword(options.Value.Password) : null;
    }
}