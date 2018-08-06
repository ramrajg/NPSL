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
        IEnumerable<FileExtension> GetFileExtenstion(int id);
        IEnumerable<Delimiter> GetDelimeterValue(int id);
        IEnumerable<TemplateGroup> GetTemplateGroupValue(int id,int OnlyActive);
        void SaveTemplate(DataTable templateItems);
        void UpdateTemplate(DataTable templateItems);
        void DeleteTemplate(int templateId);
        void SaveTemplateGroup(DataTable templateGroupItems);
        void UpdateTemplateGroup(DataTable templateGroupItems);
        void DeleteTemplateGroup(int templateGroupId);
    }
}
