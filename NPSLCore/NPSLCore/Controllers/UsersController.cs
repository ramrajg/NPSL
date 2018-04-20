using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NPSLCore.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using NPSL.Repository.API.Core.User;

namespace NPSLCore.Controllers
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _user;
        public UsersController(IUserRepository user)
        {
            _user = user;
        }

        [HttpGet]
        [Route("api/Users")]
        public IEnumerable<Users> GetUsers()
        {
            var records = _user.GetUsers();
            if (!records.Any())
            {
                //throw new HttpException(204, "GetUsers");
            }
            return records;
        }


        //[HttpGet]
        //[Route("api/Users")]
        //public IEnumerable<Usersssss> GetAll()
        //{
        //    List<Usersssss> userLst = _ctx.Users
        //             .FromSql("P_GetUser").ToList();
        //    return userLst;
        //}

        //[HttpGet]
        //[Route("api/GetUserById")]
        //public IEnumerable<Usersssss> GetUserById(int id)
        //{
        //    List<Usersssss> userLst = _ctx.Users
        //            .FromSql("P_GetUser @p0", id).ToList();
        //    return userLst;
        //}

        //[HttpPost]
        //public void Post([FromBody] Users item)
        //{
        //     _iRepo.Add(item);
        //}

        //[HttpPut]
        //public void  Update([FromBody] Users item)
        //{
        //    _iRepo.Update( item);
        //}

        //[HttpDelete]
        //[Route("api/DeleteUser")]
        //public void Delete(long id)
        //{
        //    _ctx.Database
        //   .ExecuteSqlCommand("P_DELETEUSER @p0", id);
        //}

    }
}