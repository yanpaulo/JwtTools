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
        private readonly JwtAuthenticator _jwt;

        public JwtLoginService(JwtAuthenticator jwt)
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
                RefreshTokenExpireDate = now + TimeSpan.FromDays(7),
                IssuedAt = now
            };
            user.Logins.Add(login);
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
