namespace NPSLCore.Models.DB
{
    public partial class Dashboard
    {
        public int TemplateGroupId { get; set; }
        public string TemplateGroupName { get; set; }
        public decimal ConsiledAmount { get; set; }
        public decimal NonConsiledAmount { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int TemplateSpilt { get; set; }
        
    }
}
