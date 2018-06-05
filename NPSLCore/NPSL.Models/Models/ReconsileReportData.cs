﻿
using System;
namespace NPSLCore.Models.DB
{
    public partial class ReconsileReportData
    {
        public string RRNNumber { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool IsReconsile { get; set; }
    }
}