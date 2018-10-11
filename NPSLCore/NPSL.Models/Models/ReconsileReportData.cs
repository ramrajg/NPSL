
using System;
namespace NPSLCore.Models.DB
{
    public partial class ReconsileReportData
    {
        public string RRNNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool IsReconsile { get; set; }
    }

    public partial class NonReconsileData
    {
        public int Id { get; set; }
        public string RRNNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool IsReconsile { get; set; }
        public bool IsPrimary { get; set; }
        
    }

    public class ManualResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
