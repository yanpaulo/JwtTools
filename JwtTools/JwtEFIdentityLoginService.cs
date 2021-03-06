﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yansoft.Jwt
{
    
    public class JwtEFIdentityLoginService<TUser, TUserLogin, TUserKey, TDbContext> : JwtIdentityLoginService<TUser, TUserLogin, TUserKey>, IJwtService<TUser, TUserLogin>
        where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
        where TUserKey : IEquatable<TUserKey>
        where TDbContext : DbContext
    {
        
        private readonly TDbContext _db;

        public JwtEFIdentityLoginService(IJwtAuthenticator jwt, UserManager<TUser> userManager, SignInManager<TUser> signInManager, TDbContext db) : base(jwt, userManager, signInManager)
        {
            _db = db;
        }


        public override async Task<TUserLogin> LogInAsync(TUser user, IEnumerable<string> roles)
        {
            await _db.Attach(user).Collection(u => u.UserLogins).LoadAsync();
            var login = await base.LogInAsync(user, roles);
            await _db.SaveChangesAsync();
            return login;
        }

        public override async Task<TUserLogin> LogInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            await _db.Attach(user).Collection(u => u.UserLogins).LoadAsync();
            var login = await base.LogInAsync(user, password, isPersistent, lockoutOnFailure);
            await _db.SaveChangesAsync();
            return login;
        }

        public async Task<TUserLogin> RefreshAsync(TUser user, string refreshToken)
        {
            await _db.Attach(user).Collection(u => u.UserLogins).LoadAsync();
            var now = DateTimeOffset.Now;
            var oldLogin = user.UserLogins.FirstOrDefault(l => l.RefreshToken == refreshToken && (l.RefreshTokenExpireDate == null || l.RefreshTokenExpireDate < now));
            var newLogin = await LogInAsync(user);
            user.UserLogins.Remove(oldLogin);
            await _db.SaveChangesAsync();
            return newLogin;
        }
    }

    public class JwtEFIdentityLoginService<TUser, TUserLogin, TDbContext> : JwtEFIdentityLoginService<TUser, TUserLogin, string, TDbContext>
        where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
        where TUserLogin : class, IJwtLogin, new()
        where TDbContext : DbContext
    {
        public JwtEFIdentityLoginService(JwtAuthenticator jwt, UserManager<TUser> userManager, SignInManager<TUser> signInManager, TDbContext db) : base(jwt, userManager, signInManager, db)
        {
        }
    }

    public static class JwtEFIdentityLoginServiceExtensions
    {
        public static IServiceCollection AddJwtLogin<TUser, TUserLogin, TUserKey, TDbContext>(this IServiceCollection services)
            where TUser : IdentityUser<TUserKey>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TUserKey : IEquatable<TUserKey>
            where TDbContext : DbContext
        {
            return services.AddScoped<IJwtService<TUser, TUserLogin>, JwtEFIdentityLoginService<TUser, TUserLogin, TUserKey, TDbContext>>();
        }

        public static IServiceCollection AddJwtLogin<TUser, TUserLogin, TDbContext>(this IServiceCollection services)
            where TUser : IdentityUser<string>, IJwtUser<TUserLogin>
            where TUserLogin : class, IJwtLogin, new()
            where TDbContext : DbContext
        {
            return services.AddScoped<IJwtService<TUser, TUserLogin>, JwtEFIdentityLoginService<TUser, TUserLogin, TDbContext>>();
        }
    }
}
