
using System.ComponentModel.DataAnnotations;

namespace NPSLCore.Models.DB
{
    public partial class ReconsileTemplate
    {
        public int? TemplateId { get; set; }
        [Required(ErrorMessage = "Template Name is required")]
        public string TemplateName { get; set; }
        [Required(ErrorMessage = "Folder Path is required")]
        public string SourceFolder { get; set; }
        [Required(ErrorMessage = "File Extenssion is required")]
        public string SourceExtention { get; set; }
        [Required(ErrorMessage = "Move Folder Path is required")]
        public string SourceCompletionPath { get; set; }
        public string SourceSubstringValue { get; set; }
        [Required(ErrorMessage = "Delimiter is required")]
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
