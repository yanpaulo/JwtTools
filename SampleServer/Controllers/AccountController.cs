using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data;
using Yansoft.Jwt;

namespace SampleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService<ApplicationUser, UserLogin> _jwt;

        public AccountController(IJwtService<ApplicationUser, UserLogin> jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var result = await _jwt.PasswordLogInAsync(request.UserName, request.Password, false, false);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}