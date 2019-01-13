using NPSLCore.Models.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NPSL.Repository.Core.Template
{
    public interface IReconsileTemplateRepository
    {
        IEnumerable<ReconsileTemplate> GetTemplatesById(int id);
        IEnumerable<ReconsileTemplate> GetTemplateByGroupId(int groupId);
        IEnumerable<FileExtension> GetFileExtenstion(int id);
        IEnumerable<Delimiter> GetDelimeterValue(int id);
        IEnumerable<TemplateGroup> GetTemplateGroupValue(int id,int OnlyActive);
        void SaveTemplate(DataTable templateItems);
        void UpdateTemplate(DataTable templateItems);
        void DeleteTemplate(int templateId);
        void SaveTemplateGroup(DataTable templateGroupItems);
        void UpdateTemplateGroup(DataTable templateGroupItems);
        void DeleteTemplateGroup(int templateGroupId);
        IEnumerable<Dashboard> GetDashboardData(int groupTemplateId);
        IEnumerable<ReconsileReportData> GetReconsileReportData(int groupTemplateId,int reconsileType, DateTime fromDate, DateTime toDate);
        IEnumerable<NonReconsileData> GetNonReconsileData(int groupTemplateId,int selectedTemplateId, DateTime fromDate, DateTime toDate);
        void ProcessManualReconsile(DataTable selectedResult);
        void SaveFileExtension(DataTable fileExtensionDetail);
        void UpdateFileExtension(DataTable fileExtensionDetail);
        void DeleteFileExtension(int fileExtensionId);
    }
}
