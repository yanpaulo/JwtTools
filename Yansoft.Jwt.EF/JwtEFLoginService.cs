using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Yansoft.Jwt.EF
{
    public class JwtEFLoginService<TUser, TUserLogin, TDbContext> : JwtLoginService<TUser, TUserLogin>, IJwtLoginRefreshService<TUser, TUserLogin>
        where TUser : class, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
        where TDbContext : DbContext
    {
        private readonly TDbContext _db;

        public JwtEFLoginService(JwtAuthenticator jwt, TDbContext db) : base(jwt)
        {
            _db = db;
        }

        public override async Task<TUserLogin> LogInAsync(TUser user, IEnumerable<string> roles)
        {
            var login = await base.LogInAsync(user, roles);
            await _db.SaveChangesAsync();
            return login;
        }

        public async Task<TUserLogin> RefreshAsync(TUser user, string refreshToken)
        {
            await _db.Entry(user).Collection(u => u.Logins).LoadAsync();
            var now = DateTimeOffset.Now;
            var oldLogin = user.Logins.FirstOrDefault(l => l.RefreshToken == refreshToken && (l.RefreshTokenExpireDate == null || l.RefreshTokenExpireDate < now));
            var newLogin = await LogInAsync(user);
            user.Logins.Remove(oldLogin);
            await _db.SaveChangesAsync();
            return newLogin;
        }
    }

    public static class JwtEFLoginServiceExtensions
    {
        public static IServiceCollection AddJwtLogin<TUser, TUserLogin, TDbContext>(this IServiceCollection services)
            where TUser : class, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TDbContext : DbContext
        {
            return services.AddScoped<IJwtLoginRefreshService<TUser, TUserLogin>, JwtEFLoginService<TUser, TUserLogin, TDbContext>>();
        }
    }
}
