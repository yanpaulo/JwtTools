using System;
using System.ComponentModel.DataAnnotations.Schema;
using Yansoft.Jwt;

namespace Sample.Data
{
    public class UserLogin : IJwtLogin
    {
        public int Id { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }

        [NotMapped]
        public DateTimeOffset? AccessTokenExpireDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset? RefreshTokenExpireDate { get; set; }

        public DateTimeOffset IssuedAt { get; set; }
    }
}
