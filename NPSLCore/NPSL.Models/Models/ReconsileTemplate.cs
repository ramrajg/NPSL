
namespace NPSLCore.Models.DB
{
    public partial class ReconsileTemplate
    {
        public int? TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string SourceFolder { get; set; }
        public string SourceExtention { get; set; }
        public string SourceCompletionPath { get; set; }
        public string SourceSubstringValue { get; set; }
        public string SourceDelimiter { get; set; }
        public bool? SourceHasHeader { get; set; }
        public string DestinationFolder { get; set; }
        public string DestinationExtention { get; set; }
        public string DestinationCompletionPath { get; set; }
        public string DestinationSubstringValue { get; set; }
        public string DestinationDelimiter { get; set; }
        public bool? DestinationHasHeader { get; set; }
        public bool? IsActive { get; set; }
    }
}
