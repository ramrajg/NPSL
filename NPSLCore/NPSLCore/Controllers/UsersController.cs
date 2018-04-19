using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NPSLCore.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NPSLCore.Controllers
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly NPSLContext _ctx;
        public UsersController(NPSLContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/Users")]
        public IEnumerable<Users> GetAll()
        {
            List<Users> userLst = _ctx.Users
                     .FromSql("P_GetUser").ToList();
            return userLst;
        }

        [HttpGet]
        [Route("api/GetUserById")]
        public IEnumerable<Users> GetUserById(int id)
        {
            List<Users> userLst = _ctx.Users
                    .FromSql("P_GetUser @p0", id).ToList();
            return userLst;
        }

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

        [HttpDelete]
        [Route("api/DeleteUser")]
        public void Delete(long id)
        {
            _ctx.Database
           .ExecuteSqlCommand("P_DELETEUSER @p0", id);
        }

    }
}