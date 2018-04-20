using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPSLCore.Models.DB;

namespace NPSL.Repository.API.Core.User
{
    public class UserRepository : IUserRepository
    {
       // public NPSLContext _Context { get; set; }

        private readonly NPSLContext _context;
        public UserRepository(NPSLContext context)
        {
            _context = context;
        }
        IEnumerable<Users> IUserRepository.GetUsers()
        {
            List<Users> userLst = null;
            //         .FromSql("P_GetUser").ToList();
            //return userLst;

            BaseDataAccess Data = new BaseDataAccess();
            var param = new List<DbParameter>
            {
                //new SqlParameter("@MediaId ", mediaId),
                //new SqlParameter("@AttachmentId",attachmentId)
            };

            //AttachmentRpc attachment = new AttachmentRpc();
            //MediaRpc media = new MediaRpc();

            var result = Data.GetDataReader("[dbo].[P_GetUser]", param);
            string attachmentMediaText = string.Empty;
            return userLst;
        }
    }
}
