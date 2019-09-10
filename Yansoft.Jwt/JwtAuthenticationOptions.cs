using System;
using Microsoft.IdentityModel.Tokens;

namespace Yansoft.Jwt
{
    public class JwtAuthenticationOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan? AccessTokenExpireTime { get; set; }

        public TimeSpan? RefreshTokenExpireTime { get; set; }

        public string SigningAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
    }
}
