using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yansoft.Jwt.Identity;

namespace Yansoft.Jwt
{
    public class JwtIdentityLoginService<TUser, TUserLogin, TUserKey, TDbContext> : JwtIdentityLoginService<TUser, TUserLogin, TUserKey>
        where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
        where TUserLogin : IJwtLogin, new()
        where TUserKey : IEquatable<TUserKey>
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
