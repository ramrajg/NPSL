using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NPSLCore.Contexts;
using NPSLCore.Models;

namespace NPSLCore.Repository
{
    public class UsersRepository : IUsersRepository
    {
        static List<Users> UsersList = new List<Users>();
        public void Add(Users item)
        {
            UsersList.Add(item);
        }

        public Users Find(string UserName)
        {
            return UsersList
               .Where(e => e.FirstName.Equals(UserName))
               .SingleOrDefault();
        }

        public IEnumerable<Users> GetAll()
        {
           return UsersList;
        }

        public void Remove(int UserId)
        {
            var itemToRemove = UsersList.SingleOrDefault(r => r.UserId == UserId);
            if (itemToRemove != null)
                UsersList.Remove(itemToRemove);
        }

        public void Update(Users item)
        {
            var itemToUpdate = UsersList.SingleOrDefault(r => r.UserId == item.UserId);
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
