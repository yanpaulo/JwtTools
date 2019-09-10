using System;

namespace Yansoft.Jwt
{
    public class JwtLoginResult
    {
        public JwtLoginResult(string userName, string accessToken, DateTime? expiresAt)
        {
            UserName = userName;
            AccessToken = accessToken;
            ExpiresAt = expiresAt;
        }

        public string UserName { get; private set; }

        public string AccessToken { get; private set; }

        public DateTime? ExpiresAt { get; private set; }

    }
}
