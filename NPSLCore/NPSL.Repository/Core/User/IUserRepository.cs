using NPSLCore.Models.DB;
using System.Collections.Generic;
using System.Data;

namespace NPSL.Repository.Core.User
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetUsers();
        IEnumerable<Users> GetUserById(int id);
        IEnumerable<Users> GetUsersValidation(int userId, string password);
        IEnumerable<UsersMenuModels> GetUsersMenuModel(int roleId);
        IEnumerable<MenuModelForRoleMaster> GetMenuModelForRoleMaster(int roleId);
        
        IEnumerable<Roles> GetRoleById(int roleId);
        void SaveUser(DataTable userItems);
        void UpdateUser(DataTable userItems);
        void DeleteUser(int userId);

        void UpdateRole(DataTable selectedSubMenuId, int RoleId);
        void SaveRole(DataTable selectedSubMenuId,string RoleName);
        
    }
}
