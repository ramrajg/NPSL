using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using NPSLCore.Models.DB;


namespace NPSL.Repository.Core.User
{
    public class UserRepository :IUserRepository
    {
        private readonly BaseDataAccess _DataAccess;
        public UserRepository(IConfiguration connectionstring)
        {
            _DataAccess = new BaseDataAccess(connectionstring);
        }
        IEnumerable<Users> IUserRepository.GetUsers()
        {
            var param = new List<DbParameter>
            {
                //new SqlParameter("@MediaId ", mediaId),
                //new SqlParameter("@AttachmentId",attachmentId)
            };

            List<Users> userLst = _DataAccess.ExecuteList<Users>("P_GetUser", param);
            return userLst;
        }
    }
}
