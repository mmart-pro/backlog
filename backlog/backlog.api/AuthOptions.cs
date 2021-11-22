using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backlog.api;

internal static class AuthOptions
{
    internal const string Issuer = "BacklogAuthServer"; // издатель токена
    internal const string Audience = "BacklogAuthClient"; // потребитель токена
    internal const int TokenLifeTimeHours = 8;
    internal static SymmetricSecurityKey SymmetricSecurityKey { get; private set; }

    static AuthOptions()
    {
        const string key = "ea122f22-baa8-4bb3-8075-5c09646ce941";   // ключ для шифрации
        SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}
