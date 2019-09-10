using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Yansoft.Jwt.EF
{
    public class JwtLoginService<TUser, TUserLogin, TDbContext> : JwtLoginService<TUser, TUserLogin>
        where TUser : IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
        where TDbContext : DbContext
    {
        private readonly TDbContext _db;

        public override async Task<TUserLogin> LogInAsync(TUser user, IEnumerable<string> roles)
        {
            var login = await base.LogInAsync(user, roles);
            await _db.SaveChangesAsync();
            return login;
        }
    }
}
