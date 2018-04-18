using NPSLCore.Models.DB;
using System.Collections.Generic;
using System.Linq;

namespace NPSLCore.Repository
{
    public class UsersRepository : IDataRepository<Users, long>
    {
        NPSLContext _ctx;
        public UsersRepository(NPSLContext ctx)
        {
            _ctx = ctx;
        }
        public void Add(Users item)
        {
            _ctx.Add(item);
        }

        public Users GetById(long UserId)
        {
            return _ctx.Users
               .Where(e => e.UserId.Equals(UserId))
               .SingleOrDefault();
        }

        public IEnumerable<Users> GetAll()
        {
            return _ctx.Users;
        }

        public long Delete(long UserId)
        {
            int userID = 0;
            var itemToRemove = _ctx.Users.SingleOrDefault(r => r.UserId == UserId);
            if (itemToRemove != null)
            {
                _ctx.Remove(itemToRemove);
                userID = _ctx.SaveChanges();
            }
            return userID;
        }

        public void Update(Users item)
        {
            var itemToUpdate = _ctx.Users.SingleOrDefault(r => r.UserId == item.UserId);
            if (itemToUpdate != null)
            {
                itemToUpdate.FirstName = item.FirstName;
                itemToUpdate.LastName = item.LastName;
                itemToUpdate.IsActive = item.IsActive;
                itemToUpdate.Company = item.Company;
                itemToUpdate.MobilePhone = item.MobilePhone;
                itemToUpdate.Email = item.Email;
            }
        }
    }
}
