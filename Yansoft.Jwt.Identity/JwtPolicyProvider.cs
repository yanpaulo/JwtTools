using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yansoft.Jwt
{
    public class JwtPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly IEnumerable<string> _jwtEndpoints;

        private readonly IHttpContextAccessor _httpContext;

        private readonly AuthorizationPolicy jwtPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();

        private readonly AuthorizationPolicy identityPolicy = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme).RequireAuthenticatedUser().Build();

        public JwtPolicyProvider(IHttpContextAccessor httpContext, IEnumerable<string> jwtEndpoints)
        {
            _httpContext = httpContext;
            _jwtEndpoints = jwtEndpoints;
        }

        public virtual Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            if ( _jwtEndpoints.Any(e => _httpContext.HttpContext.Request.Path.StartsWithSegments(e)))
            {
                return Task.FromResult(jwtPolicy);
            }
            return Task.FromResult(identityPolicy);
        }

        public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            throw new NotSupportedException("Named policies are not supported at this time.");
        }
    }

    public static class JwtPolicyProviderExtensions
    {
        public static IServiceCollection AddJwtPolicyProvider(this IServiceCollection services, params string[] endpoints)
        {
            return services.AddSingleton<IAuthorizationPolicyProvider>(factory => new JwtPolicyProvider(factory.GetService<IHttpContextAccessor>(), endpoints));
        }
    }
}
