using System.ComponentModel.DataAnnotations;

namespace NPSLCore.Models.DB
{
    public partial class Roles
    {
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
    
    }

    public partial class SubMenuRoleId
    {
        public int SubmenuId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }

    }
}
