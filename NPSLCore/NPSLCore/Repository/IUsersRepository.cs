
using NPSLCore.Models;
using System.Collections.Generic;
namespace NPSLCore.Repository
{
    public interface IUsersRepository
    {
        void Add(Users item);
        IEnumerable<Users> GetAll();
        Users Find(string UserName);
        void Remove(int UserId);
        void Update(Users item);
    }
}
