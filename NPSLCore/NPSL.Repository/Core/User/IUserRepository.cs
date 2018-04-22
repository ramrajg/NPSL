using NPSLCore.Models.DB;
using System.Collections.Generic;

namespace NPSL.Repository.Core.User
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetUsers();
        IEnumerable<Users> GetUserById(int id);
    }
}
