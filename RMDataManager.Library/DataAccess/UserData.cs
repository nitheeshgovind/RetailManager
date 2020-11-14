using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess sql;
        
        public UserData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<UserModel> GetUserById(string id)
        {
            var parameter = new { Id = id };

            var users = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameter, "RMData");

            return users;
        }
    }
}
