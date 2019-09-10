using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Yansoft.Jwt.Identity
{
    public class JwtIdentityLoginService<TUser, TUserLogin, TUserKey> : JwtLoginService<TUser, TUserLogin>
        where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;

        public async Task<TUserLogin> LogInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            return await LogInAsync(user);
        }

        public override async Task<TUserLogin> LogInAsync(TUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return await LogInAsync(user, roles);
        }
    }
}
