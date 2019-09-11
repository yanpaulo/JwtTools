using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Yansoft.Jwt.Identity
{
    
    public class JwtIdentityLoginService<TUser, TUserLogin, TUserKey> : JwtLoginService<TUser, TUserLogin>
        where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
        where TUserKey : IEquatable<TUserKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;

        public JwtIdentityLoginService(JwtAuthenticator jwt, UserManager<TUser> userManager, SignInManager<TUser> signInManager) : base(jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public override async Task<TUserLogin> LogInAsync(TUser user)
        {
            if (_userManager.SupportsUserRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return await LogInAsync(user, roles);
            }
            return await base.LogInAsync(user);
        }

        public async Task<TUserLogin> LogInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            if (!result.Succeeded)
            {
                var exception = new InvalidOperationException($"Login failed for user {user.UserName}");
                exception.Data[nameof(user)] = user;
                exception.Data[nameof(result)] = result;
                throw exception;
            }
            return await LogInAsync(user);
        }
    }

    public class JwtIdentityLoginService<TUser, TUserLogin> : JwtIdentityLoginService<TUser, TUserLogin, string>
        where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
    {
        public JwtIdentityLoginService(JwtAuthenticator jwt, UserManager<TUser> userManager, SignInManager<TUser> signInManager) : base(jwt, userManager, signInManager)
        {
        }
    }

    public static class JwtIdentityLoginServiceExtensions
    {
        public static IServiceCollection AddJwtLogin<TUser, TUserLogin>(this IServiceCollection services)
            where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
            where TUserLogin : IJwtLogin, new()
        {
            return services.AddScoped<IJwtLoginService<TUser, TUserLogin>, JwtIdentityLoginService<TUser, TUserLogin>>();

        }

        public static IServiceCollection AddJwtLogin<TUser, TUserLogin, TUserKey>(this IServiceCollection services)
            where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
            where TUserLogin : IJwtLogin, new()
            where TUserKey : IEquatable<TUserKey>
        {
            return services.AddScoped<IJwtLoginService<TUser, TUserLogin>, JwtIdentityLoginService<TUser, TUserLogin, TUserKey>>();
        }
    }
}
