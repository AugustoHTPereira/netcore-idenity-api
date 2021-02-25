using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Supermercado.Controllers.Base
{
    public class Controller : ControllerBase
    {
        public string UserId { get { return User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value!; } }
    }
}
