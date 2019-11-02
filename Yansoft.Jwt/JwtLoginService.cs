using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yansoft.Jwt;

namespace Yansoft.Jwt
{
    public class JwtLoginService<TUser, TUserLogin> : IJwtLoginService<TUser, TUserLogin> where TUser : IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
    {
        private readonly IJwtAuthenticator _jwt;

        public JwtLoginService(IJwtAuthenticator jwt)
        {
            _jwt = jwt;
        }

        public virtual Task<TUserLogin> LogInAsync(TUser user)
        {
            return LogInAsync(user, new string[0]);
        }

        public virtual Task<TUserLogin> LogInAsync(TUser user, IEnumerable<string> roles)
        {
            var jwtResult = _jwt.LogIn(user.UserName, roles);

            var now = DateTimeOffset.Now;
            var login = new TUserLogin
            {
                UserName = user.UserName,
                AccessToken = jwtResult.AccessToken,
                AccessTokenExpireDate = jwtResult.ExpiresAt,
                RefreshToken = GenerateRefreshToken(64),
                RefreshTokenExpireDate = jwtResult.ExpiresAt + _jwt.Options.RefreshTokenExpireTime,
                IssuedAt = jwtResult.IssuedAt
            };
            user.UserLogins?.Add(login);
            return Task.FromResult(login);
        }

        protected virtual string GenerateRefreshToken(int length)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                return Regex.Replace(Convert.ToBase64String(bytes), @"[^\w]", "");
            }
        }
    }
}
