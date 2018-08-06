
using System.ComponentModel.DataAnnotations;

namespace NPSLCore.Models.DB
{
    public partial class TemplateGroup
    {
        public int? TemplateGroupId { get; set; }
        [Required(ErrorMessage = "Template Name is required")]
        public string TemplateGroupName { get; set; }
        public bool? IsTemplateGroupActive { get; set; }
    }
}
