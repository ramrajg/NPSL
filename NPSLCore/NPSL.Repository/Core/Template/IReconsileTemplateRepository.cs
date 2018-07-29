using NPSLCore.Models.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPSL.Repository.Core.Template
{
    public interface IReconsileTemplateRepository
    {
        IEnumerable<ReconsileTemplate> GetTemplatesById(int id);
        IEnumerable<FileExtension> GetFileExtenstion(int id);
        IEnumerable<Delimiter> GetDelimeterValue(int id);
    }
}
