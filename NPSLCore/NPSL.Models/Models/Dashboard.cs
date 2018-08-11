namespace NPSLCore.Models.DB
{
    public partial class Dashboard
    {
        public int TemplateGroupId { get; set; }
        public string TemplateGroupName { get; set; }
        public int ConsiledAmount { get; set; }
        public int NonConsiledAmount { get; set; }
    }
}
