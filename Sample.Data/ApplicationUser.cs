using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text;
using Yansoft.Jwt;

namespace Sample.Data
{
    public class ApplicationUser : IdentityUser, IJwtUser<UserLogin>
    {
        public IList<UserLogin> UserLogins { get; set; }
    }
}
