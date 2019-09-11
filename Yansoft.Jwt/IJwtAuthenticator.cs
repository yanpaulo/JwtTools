using System.Collections.Generic;

namespace Yansoft.Jwt
{
    public interface IJwtAuthenticator
    {
        JwtLoginResult LogIn(string userName);
        JwtLoginResult LogIn(string userName, IEnumerable<string> roles);
        JwtLoginResult LogIn(string userName, IEnumerable<string> roles, JwtAuthenticationOptions options);
    }
}