using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Controllers.RequestTypes.Account;
using Supermercado.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controller = Supermercado.Controllers.Base.Controller;

namespace Supermercado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/<AccountController>
        [HttpGet, Authorize("Bearer")]
        public async Task<IActionResult> Get()
        {
            var result = await _userManager.FindByIdAsync(UserId);

            if (result != null)
                return Ok(result);

            return NotFound();
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterRequest request)
        {
            var user = new User();
            user.Email = request.Email;
            user.EmailConfirmed = true;
            user.UserName = request.Email;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return Ok(await _userManager.GetUserIdAsync(user));

            return BadRequest(result.Errors);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] EditRequest request)
        {
            var user = new User();
            user.Id = id;
            user.FullName = request.Name;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
