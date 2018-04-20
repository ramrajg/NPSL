using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPSLCore.Models.DB;

namespace NPSL.Repository.API.Core.User
{
    public class UserRepository : IUserRepository
    {
        IEnumerable<Users> IUserRepository.GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
