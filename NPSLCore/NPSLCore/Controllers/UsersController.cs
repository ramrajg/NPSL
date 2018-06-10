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
using System.Reflection;
using System.Data;

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
        public IEnumerable<UsersMenuModels> GetUsersMenuModel(int roleId)
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

        [HttpGet]
        [Route("api/GetRoleById")]
        public IEnumerable<Roles> GetRoleById(int roleId)
        {
            try
            {
                var records = _user.GetRoleById(roleId);
                return records;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        [HttpPost]
        [Route("api/SaveUser")]
        public void SaveUser([FromBody] Users userDetail)
        {
            try
            {
                var usr = new DataTable();
                usr.Columns.Add("FirstName", typeof(string));
                usr.Columns.Add("LastName", typeof(string));
                usr.Columns.Add("IsActive", typeof(bool));
                usr.Columns.Add("Company", typeof(string));
                usr.Columns.Add("LoginId", typeof(int));
                usr.Columns.Add("LoginPassword", typeof(string));
                usr.Columns.Add("Email", typeof(string));
                usr.Columns.Add("MobilePhone", typeof(string));
                usr.Columns.Add("RoleId", typeof(int));

                DataRow newRow = usr.Rows.Add();
                newRow["FirstName"] = userDetail.FirstName;
                newRow["LastName"] = userDetail.LastName;
                newRow["IsActive"] = userDetail.IsActive;
                newRow["Company"] = userDetail.Company;
                newRow["LoginId"] = userDetail.UserId;
                newRow["LoginPassword"] = userDetail.UserPassword;
                newRow["Email"] = userDetail.Email;
                newRow["MobilePhone"] = userDetail.MobilePhone;
                newRow["RoleId"] = userDetail.RoleId;


                _user.SaveUser(usr);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }

        //[HttpGet]
        //[Route("api/GetUsersValidationMenuModel")]
        //public IEnumerable<UsersMenuModels> GetUsersValidationMenuModel(int userId, string password)
        //{
        //    try
        //    {
        //        var records = _user.GetUsersValidationMenuModel(userId, password);
        //        return records;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new CustomException(ex.Message.ToString());
        //    }
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

        [HttpPost]
        [Route("api/DeleteUser")]
        public void DeleteUser(int id)
        {
            _user.DeleteUser(id);
        }

    }
}