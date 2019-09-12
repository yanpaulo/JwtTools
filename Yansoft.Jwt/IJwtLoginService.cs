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
    public interface IJwtPasswordLoginService<TUser, TUserLogin> : IJwtLoginService<TUser, TUserLogin>
        where TUser : IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
    {
        Task<TUserLogin> PasswordLogInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    }

    public interface IJwtLoginRefreshService<TUser, TUserLogin> : IJwtLoginService<TUser, TUserLogin>
        where TUser : class, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
    {
        Task<TUserLogin> RefreshAsync(TUser user, string refreshToken);
    }

    public interface IJwtService<TUser, TUserLogin> : IJwtPasswordLoginService<TUser, TUserLogin>, IJwtLoginRefreshService<TUser, TUserLogin>
        where TUser : class, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
    {

    }

}