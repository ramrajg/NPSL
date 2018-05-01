using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NPSL.Models.Models.DB;
using NPSLCore.Models.DB;


namespace NPSL.Repository.Core.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _DBContext;
        public UserRepository(DatabaseContext dbcontext)
        {
            _DBContext = dbcontext;
        }
        IEnumerable<Users> IUserRepository.GetUsers()
        {
            List<Users> userLst = _DBContext.ExecuteTransactional<Users>("P_GetUser");
            return userLst;
        }
        IEnumerable<Users> IUserRepository.GetUserById(int id)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@UserId ", id),
            };

            List<Users> userLst = _DBContext.ExecuteTransactional<Users>("P_GetUser", param);
            return userLst;
        }
        IEnumerable<Users> IUserRepository.GetUsersValidation(int userId, string password)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@UserId ", userId),
                new SqlParameter("@Password ", password),
            };

            List<Users> userLst = _DBContext.ExecuteTransactional<Users>("P_GETUSERSVALIDATION", param);
            return userLst;
        }
    }
}
