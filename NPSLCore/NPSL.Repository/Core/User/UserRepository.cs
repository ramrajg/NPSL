using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        IEnumerable<UsersMenuModels> IUserRepository.GetUsersMenuModel(int roleId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@RoleID ", roleId),
            };

            List<UsersMenuModels> userMenu = _DBContext.ExecuteTransactional<UsersMenuModels>("P_GETMENUBYROLEID", param);
            return userMenu;
        }

        IEnumerable<MenuModelForRoleMaster> IUserRepository.GetMenuModelForRoleMaster(int roleId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@RoleID ", roleId),
            };

            List<MenuModelForRoleMaster> userMenu = _DBContext.ExecuteTransactional<MenuModelForRoleMaster>("P_GetMenuModelForRoleMaster", param);
            return userMenu;
        }

        IEnumerable<Roles> IUserRepository.GetRoleById(int roleId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@RoleID ", roleId),
            };

            List<Roles> roleById = _DBContext.ExecuteTransactional<Roles>("P_GETROLEBYID", param);
            return roleById;
        }

        public void UpdateRole(DataTable selectedSubMenuId, int RoleId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pSelectedMenuId",
                SqlDbType = SqlDbType.Structured,
                Value = selectedSubMenuId,
                TypeName = "udt_selectedMenuId"
            }, new SqlParameter("@pRoleId", RoleId),
        
            };

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_UpdateRole", param);
        }

        public void SaveRole(DataTable selectedSubMenuId, string RoleName)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pSelectedMenuId",
                SqlDbType = SqlDbType.Structured,
                Value = selectedSubMenuId,
                TypeName = "udt_selectedMenuId"
            }, new SqlParameter("@pRoleName", RoleName),
            };

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_SaveRole", param);
        }

        void IUserRepository.SaveUser(DataTable userItems)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pUser",
                SqlDbType = SqlDbType.Structured,
                Value = userItems,
                TypeName = "udt_users"
            }
            };

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_SAVEUSERS", param);
        }
        void IUserRepository.UpdateUser(DataTable userItems)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pUser",
                SqlDbType = SqlDbType.Structured,
                Value = userItems,
                TypeName = "udt_users"
            }
            };

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_UPDATEUSERS", param);
        }
        void IUserRepository.DeleteUser(int userId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@pUserId ", userId),
            };
            var Data = _DBContext.ExecuteTransactionalNonQuery("P_DELETEUSER", param);
        }

    }
}
