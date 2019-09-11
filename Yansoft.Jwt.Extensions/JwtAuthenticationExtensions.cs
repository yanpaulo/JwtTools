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
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtAuthenticationOptions options, Action<JwtBearerOptions> configureOptions)
        {
            var optionsInstance = new JwtBearerOptions();
            configureOptions(optionsInstance);
            var jwtService = new JwtAuthenticator(optionsInstance.TokenValidationParameters, options);

            services.AddAuthentication().AddJwtBearer(configureOptions);
            return services.AddSingleton<IJwtAuthenticator>(jwtService);
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtAuthenticationOptions options, TokenValidationParameters parameters)
        {

            return services.AddJwtAuthentication(options, jwt =>
            {
                jwt.TokenValidationParameters = parameters;
            });
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtAuthenticationOptions options)
        {
            var key = System.Text.Encoding.ASCII.GetBytes(options.Secret);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return services.AddJwtAuthentication(options, parameters);
        }

        
        #endregion

        #region Utility Overloads
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, Action<JwtBearerOptions> configureOptions, string configurationSection = DefaultConfigurationSection)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IConfiguration>().GetSection(configurationSection).Get<JwtAuthenticationOptions>();

            return services.AddJwtAuthentication(options, configureOptions);
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenValidationParameters parameters, string configurationSection = DefaultConfigurationSection)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IConfiguration>().GetSection(configurationSection).Get<JwtAuthenticationOptions>() ?? new JwtAuthenticationOptions();

            return services.AddJwtAuthentication(options, parameters);
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string configurationSection = DefaultConfigurationSection)
        {
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IConfiguration>().GetSection(configurationSection).Get<JwtAuthenticationOptions>() ?? new JwtAuthenticationOptions();

            return services.AddJwtAuthentication(options);
        }
        #endregion



    }

    #endregion

}
