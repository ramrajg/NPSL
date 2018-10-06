using NPSLCore.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPSLWeb.Models
{
    public class ViewModel
    {
        public IEnumerable<ReconsileReportData> ReportData { get; set; }
        public IEnumerable<TemplateGroup> TemplateGroup { get; set; }
    }

    public class ViewModelNonReconsile
    {
        public IEnumerable<NonReconsileData> NonReconsileData { get; set; }
        public IEnumerable<TemplateGroup> TemplateGroup { get; set; }
    }
}
