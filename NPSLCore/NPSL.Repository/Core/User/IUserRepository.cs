using NPSLCore.Models.DB;
using System.Collections.Generic;

namespace NPSL.Repository.API.Core.User
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetUsers();
    }
}
