using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Yansoft.Jwt
{
    public class JwtAuthenticator
    {
        public JwtAuthenticator(TokenValidationParameters tokenValidationParameters, JwtAuthenticationOptions options)
        {
            TokenValidationParameters = tokenValidationParameters;
            Options = options;
        }

        public JwtAuthenticationOptions Options { get; private set; }

        public TokenValidationParameters TokenValidationParameters { get; private set; }

        public SecurityKey SigningKey => TokenValidationParameters.IssuerSigningKey;

        public SigningCredentials SigningCredentials => new SigningCredentials(SigningKey, Options.SigningAlgorithm);

        public JwtLoginResult LogIn(string userName) =>
            LogIn(userName, new string[0]);

        public JwtLoginResult LogIn(string userName, IEnumerable<string> roles) =>
            LogIn(userName, roles, Options.AccessTokenExpireTime);

        public JwtLoginResult LogIn(string userName, IEnumerable<string> roles, TimeSpan? expireTime)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Options.Issuer,
                Audience = Options.Audience,
                IssuedAt = DateTime.UtcNow,
                Expires = expireTime.HasValue ? DateTime.UtcNow + expireTime : null,
                SigningCredentials = new SigningCredentials(SigningKey, Options.SigningAlgorithm)
            };
            
            var token = handler.CreateToken(descriptor);
            return new JwtLoginResult(userName, handler.WriteToken(token), descriptor.Expires);
        }

        public string ReadUserName(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(token);

            return securityToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value;
        }
    }

}
