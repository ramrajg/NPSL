using NPSL.Models.Models.DB;
using NPSLCore.Models.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NPSL.Topography
{
    public class usersFunctions
    {
        private readonly DatabaseContext _DBContext;
        //public UsersMenuModels GetUsersValidationMenuModel(int userId, string password)
        //{

        //    var retVal = new UsersMenuModels();
        //    var param = new List<SqlParameter>
        //    {
        //        new SqlParameter("@UserId ", userId),
        //        new SqlParameter("@Password ", password),
        //    };

        //    List<UsersMenuModels> userMenu = _DBContext.ExecuteTransactional<UsersMenuModels>("P_GETUSERSVALIDATIONMENUMODEL", param);

        //    return userMenu;
        //}
     
    }
}
