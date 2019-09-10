using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Yansoft.Jwt
{
    #region Extension Methods

    public static class JwtAuthenticationExtensions
    {
        public const string DefaultConfigurationSection = "JWT";

        #region Main Implementations
        public static void AddJwtAuthentication(this IServiceCollection services, Action<JwtBearerOptions> configureOptions, JwtAuthenticationOptions options)
        {
            var optionsInstance = new JwtBearerOptions();
            configureOptions(optionsInstance);
            var jwtService = new JwtAuthenticator(optionsInstance.TokenValidationParameters, options);

            services.AddAuthentication().AddJwtBearer(configureOptions);
            services.AddSingleton(jwtService);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, JwtAuthenticationOptions options)
        {
            var key = System.Text.Encoding.ASCII.GetBytes(options.Secret);

            AddJwtAuthentication(services, jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            }, options);
        }
        #endregion

        #region Utility Overloads
        public static void AddJwtAuthentication(this IServiceCollection services, Action<JwtBearerOptions> configureOptions, string configurationSection = DefaultConfigurationSection)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IConfiguration>().GetSection(configurationSection).Get<JwtAuthenticationOptions>();
            AddJwtAuthentication(services, configureOptions, options);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, string configurationSection = DefaultConfigurationSection)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IConfiguration>().GetSection(configurationSection).Get<JwtAuthenticationOptions>() ?? new JwtAuthenticationOptions();

            AddJwtAuthentication(services, options);
        }
        #endregion



    }

    #endregion

}
