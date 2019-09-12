using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yansoft.Jwt
{
    public static class JwtToolsExtensions
    {
        public static IServiceCollection AddJwtTools<TUser, TUserLogin, TDbContext>(this IServiceCollection services)
            where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TDbContext : DbContext
        {
            return services.AddJwtTools<TUser, TUserLogin, string, TDbContext>();
        }

        public static IServiceCollection AddJwtTools<TUser, TUserLogin, TDbContext>(this IServiceCollection services, params string[] jwtEndpoints)
            where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TDbContext : DbContext
        {
            return services.AddJwtTools<TUser, TUserLogin, string, TDbContext>(jwtEndpoints);
        }

        public static IServiceCollection AddJwtTools<TUser, TUserLogin, TUserKey, TDbContext>(this IServiceCollection services)
            where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TUserKey : IEquatable<TUserKey>
            where TDbContext : DbContext
        {
            return services.AddJwtTools<TUser, TUserLogin, TUserKey, TDbContext>(new string[0]);
        }

        public static IServiceCollection AddJwtTools<TUser, TUserLogin, TUserKey, TDbContext>(this IServiceCollection services, params string[] jwtEndpoints)
            where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TUserKey : IEquatable<TUserKey>
            where TDbContext : DbContext
        {
            services.AddJwtAuthentication();
            services.AddJwtLogin<TUser, TUserLogin, TUserKey, TDbContext>();
            if (jwtEndpoints.Any())
            {
                services.AddJwtPolicyProvider(jwtEndpoints);
            }
            return services;
        }
    }
}
