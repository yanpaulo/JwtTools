using System;

namespace Yansoft.Jwt
{
    public interface IJwtLogin
    {
        string UserName { get; set; }
        string AccessToken { get; set; }
        DateTimeOffset? AccessTokenExpireDate { get; set; }
        string RefreshToken { get; set; }
        DateTimeOffset? RefreshTokenExpireDate { get; set; }
        DateTimeOffset IssuedAt { get; set; }
    }
}
