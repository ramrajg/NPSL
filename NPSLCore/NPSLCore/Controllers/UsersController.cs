﻿using Microsoft.AspNetCore.Mvc;
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
        [Route("api/GetMenuModelForRoleMaster")]
        public IEnumerable<MenuModelForRoleMaster> GetMenuModelForRoleMaster(int roleId)
        {
            try
            {
                var records = _user.GetMenuModelForRoleMaster(roleId);
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
        [Route("api/UpdateRole")]
        public void UpdateRole([FromBody] List<SubMenuRoleId> selectedMenuId)
        {
            try
            {
                var subMenuId = new DataTable();
                subMenuId.Columns.Add("SubMenuId", typeof(int));
                selectedMenuId.ForEach((item) => subMenuId.Rows.Add(item.SubmenuId));

                int RoleId = selectedMenuId.Select(x => x.RoleId).Distinct().FirstOrDefault();


                _user.UpdateRole(subMenuId, RoleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/SaveRole")]
        public void SaveRole([FromBody] List<SubMenuRoleId> selectedMenuId)
        {
            try
            {
                var subMenuId = new DataTable();
                subMenuId.Columns.Add("SubMenuId", typeof(int));
                selectedMenuId.ForEach((item) => subMenuId.Rows.Add(item.SubmenuId));

                string RoleName = selectedMenuId.Select(x => x.RoleName).Distinct().FirstOrDefault();
    
                _user.SaveRole(subMenuId, RoleName);
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

        [HttpPost]
        [Route("api/UpdateUser")]
        public void UpdateUser([FromBody] Users userDetail)
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


                _user.UpdateUser(usr);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message.ToString());
            }
        }
        



       [HttpPost]
        [Route("api/DeleteUser")]
        public void DeleteUser([FromBody] int Id)
        {
            _user.DeleteUser(Id);
        }

    }
}