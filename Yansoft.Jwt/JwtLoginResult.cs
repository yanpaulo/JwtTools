using System;

namespace Yansoft.Jwt
{
    public class JwtLoginResult
    {
        public JwtLoginResult(string userName, string accessToken, DateTimeOffset issuedAt, DateTimeOffset? expiresAt)
        {
            UserName = userName;
            AccessToken = accessToken;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }

        public string UserName { get; private set; }

        public string AccessToken { get; private set; }

        public DateTimeOffset IssuedAt { get; set; }

        public DateTimeOffset? ExpiresAt { get; private set; }

    }
}
