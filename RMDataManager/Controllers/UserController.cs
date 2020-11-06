using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public UserModel GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            var users = data.GetUserById(id);
            return users.First();
        }

        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> applicationUsers = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel _user = new ApplicationUserModel()
                    {
                        Id = user.Id,
                        EmailAddress = user.Email
                    };
                    foreach (var userRole in user.Roles)
                    {
                        _user.Roles.Add(userRole.RoleId, roles.First(x => x.Id == userRole.RoleId).Name);
                    }
                    applicationUsers.Add(_user);
                }
            }
            return applicationUsers;
        }
    }
}