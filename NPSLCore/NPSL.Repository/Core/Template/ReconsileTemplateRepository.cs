using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NPSL.Models.Models.DB;
using NPSLCore.Models.DB;

namespace NPSL.Repository.Core.Template
{
    public class ReconsileTemplateRepository : IReconsileTemplateRepository
    {
        private readonly DatabaseContext _DBContext;

        public ReconsileTemplateRepository(DatabaseContext dbcontext)
        {
            _DBContext = dbcontext;
        }

        public IEnumerable<FileExtension> GetFileExtenstion(int id)
        {
            List<FileExtension> FileExtensionLst = _DBContext.ExecuteTransactional<FileExtension>("P_GETFILEEXTENSION");
            return FileExtensionLst;
        }

        public IEnumerable<ReconsileTemplate> GetTemplatesById(int id)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@TemplateId ", id),
            };
            List<ReconsileTemplate> ReconsileTemplateLst = _DBContext.ExecuteTransactional<ReconsileTemplate>("P_GETTEMPLATE", param);
            return ReconsileTemplateLst;
        }
    }
}
