
using System;

namespace NPSLCore.Models.DB
{
    public partial class NonReconsileData
    {
        public string RRNNumber { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
    }
}
