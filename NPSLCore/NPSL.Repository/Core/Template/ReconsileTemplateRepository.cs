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


        public IEnumerable<TemplateGroup> GetTemplateGroupValue(int id, int OnlyActive)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@TEMPLATEGROUPID", id),
                new SqlParameter("@OnlyActive", OnlyActive),
            };
            List<TemplateGroup> TemplateGroupValueLst = _DBContext.ExecuteTransactional<TemplateGroup>("P_GETTEMPLATEGROUPVALUE", param);
            return TemplateGroupValueLst;
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

        public void UpdateTemplate(DataTable templateItems)
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

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_UpdateTemplate", param);
        }

        public void DeleteTemplate(int templateId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@pTemplateId ", templateId),
            };
            var Data = _DBContext.ExecuteTransactionalNonQuery("P_DELETETEMPLATE", param);
        }
        void IReconsileTemplateRepository.SaveTemplateGroup(DataTable templateGroupItems)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pTemplateGroup",
                SqlDbType = SqlDbType.Structured,
                Value = templateGroupItems,
                TypeName = "udt_templateGroup"
            }
            };
            var Data = _DBContext.ExecuteTransactionalNonQuery("P_SaveTemplateGroup", param);
        }

        public void UpdateTemplateGroup(DataTable templateGroupItems)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter {
                ParameterName = "@pTemplateGroup",
                SqlDbType = SqlDbType.Structured,
                Value = templateGroupItems,
                TypeName = "udt_templateGroup"
            }
            };

            var Data = _DBContext.ExecuteTransactionalNonQuery("P_UpdateTemplateGroup", param);
        }

        public void DeleteTemplateGroup(int templateGroupId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@pTemplateGroupId ", templateGroupId),
            };
            var Data = _DBContext.ExecuteTransactionalNonQuery("P_DELETETEMPLATEGroup", param);
        }

        public IEnumerable<Dashboard> GetDashboardData(int groupTemplateId)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@GroupTemplateId ", groupTemplateId),
            };
            List<Dashboard> DashboardData = _DBContext.ExecuteTransactional<Dashboard>("P_GetDashboardData", param);
            return DashboardData;
        }

        public IEnumerable<ReconsileReportData> GetReconsileReportData(int groupTemplateId, DateTime fromDate, DateTime toDate)
        {
            var param = new List<SqlParameter>
            {
                new SqlParameter("@GroupTemplateId", groupTemplateId),
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
            };
            List<ReconsileReportData> ReconsileReportData = _DBContext.ExecuteTransactional<ReconsileReportData>("P_GetReconsileReportData", param);
            return ReconsileReportData;
        }
    }
}
