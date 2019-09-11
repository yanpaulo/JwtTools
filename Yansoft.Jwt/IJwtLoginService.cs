using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yansoft.Jwt
{
    public interface IJwtLoginService<TUser, TUserLogin>
        where TUser : IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
    {
        Task<TUserLogin> LogInAsync(TUser user);
        Task<TUserLogin> LogInAsync(TUser user, IEnumerable<string> roles);
    }
    public interface IJwtLoginRefreshService<TUser, TUserLogin> : IJwtLoginService<TUser, TUserLogin>
        where TUser : class, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
    {
        Task<TUserLogin> RefreshAsync(TUser user, string refreshToken);
    }

}