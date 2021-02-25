using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Controllers.ResponseTypes.Login
{
    public class LoginResponse
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiressAt { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
