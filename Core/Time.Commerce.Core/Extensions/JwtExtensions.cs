using Microsoft.IdentityModel.Tokens;
using System.Text;
using Time.Commerce.Core.Options;

namespace Time.Commerce.Core.Extensions
{
    public static class JwtExtensions
    {
        public static TokenValidationParameters GenTokenValidationParameters(AuthOptions audience)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience.Secret));
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audience.Iss,
                ValidateAudience = true,
                ValidAudience = audience.Aud,
                ValidateLifetime = true,//here we are saying that we don't care or care about the token's expiration date
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }
    }
}
