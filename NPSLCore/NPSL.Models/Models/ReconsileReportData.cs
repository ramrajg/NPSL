
using System;
namespace NPSLCore.Models.DB
{
    public partial class ReconsileReportData
    {
        public string PrimaryRRNNumber { get; set; }
        public decimal PrimaryAmount { get; set; }
        public DateTime PrimaryDate { get; set; }
        public int PrimaryTemplateId { get; set; }
        public string PrimaryTemplateName { get; set; }
        public bool PrimaryIsReconsile { get; set; }
        public string PrimaryReconsileType { get; set; }
        public string PrimaryReconsileDesc { get; set; }
        public string NonPrimaryRRNNumber { get; set; }
        public decimal NonPrimaryAmount { get; set; }
        public DateTime NonPrimaryDate { get; set; }
        public int NonPrimaryTemplateId { get; set; }
        public string NonPrimaryTemplateName { get; set; }
        public bool NonPrimaryIsReconsile { get; set; }
        public string NonPrimaryReconsileType { get; set; }
        public string NonPrimaryReconsileDesc { get; set; }
        public Int64 denseCount { get; set; }
    }

    public partial class NonReconsileData
    {
        public int Id { get; set; }
        public string RRNNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public int TemplateSpilt { get; set; }
        public string TemplateName { get; set; }
        public bool IsReconsile { get; set; }
        public bool IsPrimary { get; set; }
        public string ReconsileType { get; set; }
        public string ReconsileDesc { get; set; }

    }

    public class ManualResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ReasonDesc { get; set; }
    }
}
