using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMApi.Models;
using RMApi.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        
        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserData data = new UserData(_configuration);
            var users = data.GetUserById(id);
            return users.First();
        }

        [HttpGet]
        [Route("Admin/GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> applicationUsers = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new
                            {
                                ur.UserId,
                                r.Id,
                                r.Name
                            };
            
            foreach (var user in users)
            {
                ApplicationUserModel _user = new ApplicationUserModel()
                {
                    Id = user.Id,
                    EmailAddress = user.Email,
                    Roles = userRoles.Where(x => x.UserId == user.Id).ToDictionary(x => x.Id, x => x.Name)
                };
                applicationUsers.Add(_user);
            }

            return applicationUsers;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin/getallroles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("admin/addrole")]
        public async Task AddRole(UserRoleModel role)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == role.UserId);
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("admin/removerole")]
        public async Task RemoveRole(UserRoleModel role)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == role.UserId);
            await _userManager.RemoveFromRoleAsync(user, role.Name);
        }
    }
}