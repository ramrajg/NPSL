using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NPSLCore.Models.DB;
using System.Linq;
using NPSL.Repository.Core.User;
using System;
using System.Net;
using NPSL.Extensions;
using NPSL.Extensions.CustomException;
using NPSL.Extentions.CustomException;
using Microsoft.AspNetCore.Cors;

namespace NPSLCore.Controllers
{
    [Produces("application/json")]
    [EnableCors("AllowAllHeaders")]
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
                const string msg = "User does not exsists";
                throw new CustomException(msg);
            }
            return records;
        }

        [HttpGet]
        [Route("api/GetUserById")]
        public IEnumerable<Users> GetUserById(int id)
        {
            var records = _user.GetUserById(id);
            if (!records.Any())
            {
                const string msg = "User does not exsists";
                throw new Exception(msg);
            }
            return records;
        }

        [HttpGet]
        [Route("api/GetUsersValidation")]
        public IEnumerable<Users> GetUsersValidation(int userId, string password)
        {
            try
            {
                var records = _user.GetUsersValidation(userId, password);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpGet]
        [Route("api/GetUsersMenuModel")]
        public IEnumerable<MenuModels> GetUsersMenuModel(int roleId)
        {
            try
            {
                var records = _user.GetUsersMenuModel(roleId);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
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

        //[HttpDelete]
        //[Route("api/DeleteUser")]
        //public void Delete(long id)
        //{
        //    _ctx.Database
        //   .ExecuteSqlCommand("P_DELETEUSER @p0", id);
        //}

    }
}