using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class UserData
    {
        Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public UserData(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);
            var parameter = new { Id = id };

            var users = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameter, "RMData");

            return users;
        }
    }
}
