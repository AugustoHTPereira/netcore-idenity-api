using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Supermercado.Controllers.RequestTypes.Login;
using Supermercado.Controllers.ResponseTypes.Login;
using Supermercado.Models;
using Controller = Supermercado.Controllers.Base.Controller;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supermercado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly TokenSettings _tokenSettings;
        private readonly SigningConfiguration _signingConfiguration;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, TokenSettings tokenSettings, SigningConfiguration signingConfiguration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenSettings = tokenSettings;
            _signingConfiguration = signingConfiguration;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return NotFound();

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, true, true);
            if (!result.Succeeded)
                return Unauthorized();

            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity("Id", user.Id),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    }
                );

            var createdAt = DateTime.Now;
            var expiress = createdAt.AddHours(_tokenSettings.Expiress);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expiress
            });

            var accessToken = handler.WriteToken(securityToken);

            return Ok(new LoginResponse
            {
                CreatedAt = createdAt,
                ExpiressAt = expiress,
                Token = accessToken,
                UserId = user.Id
            });
        }
    }
}
