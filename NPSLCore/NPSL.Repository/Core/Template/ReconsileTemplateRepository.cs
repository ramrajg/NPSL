using System;
using System.Collections.Generic;
using System.Data;
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
            var param = new List<SqlParameter>
            {
                new SqlParameter("@EXTENSTIONID ", id),
            };
            List<FileExtension> FileExtensionLst = _DBContext.ExecuteTransactional<FileExtension>("P_GETFILEEXTENSION", param);
            return FileExtensionLst;
        }

        public IEnumerable<Delimiter> GetDelimeterValue(int id)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@DELIMITERID ", id),
            };
            List<Delimiter> DelimeterValueLst = _DBContext.ExecuteTransactional<Delimiter>("P_GETDELIMETERVALUE", param);
            return DelimeterValueLst;
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
        void IReconsileTemplateRepository.SaveTemplate(DataTable templateItems)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pTemplate",
                SqlDbType = SqlDbType.Structured,
                Value = templateItems,
                TypeName = "udt_template"
            }
            };
            var Data = _DBContext.ExecuteTransactionalNonQuery("P_SaveTemplate", param);
        }

    }
}
