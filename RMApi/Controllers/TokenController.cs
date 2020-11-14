using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RMApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public TokenController(ApplicationDbContext context,
                                UserManager<IdentityUser> userManager,
                                IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string userName, string password, string grant_type)
        {
            if (await IsValidUserAndPassword(userName, password))
            {
                return Ok(await GenerateToken(userName));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUserAndPassword(string username, string password)
        {
            var user = await userManager.FindByEmailAsync(username);
            return await userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            // Get the user
            var user = await userManager.FindByEmailAsync(username);
            
            // Get all the roles for the current user
            var roles = from ur in context.UserRoles
                        join r in context.Roles
                        on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, r.Id, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secrets:SecurityKey"]));

            // Singing Credentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create JWT token
            var token = new JwtSecurityToken(
                        null,
                        null,
                        claims,
                        expires: DateTime.Now.AddHours(2),
                        signingCredentials: credentials);

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return output;
        }
    }
}