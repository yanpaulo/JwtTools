using System.Collections.Generic;

namespace Yansoft.Jwt
{
    public interface IJwtUser<T> where T : IJwtLogin
    {
        string UserName { get; set; }
        IList<T> Logins { get; set; }
    }
}
