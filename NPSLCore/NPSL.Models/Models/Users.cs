using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPSLCore.Models.DB
{
    public partial class Users
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public string Company { get; set; }
        [Required(ErrorMessage = "Email Id is required")]
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [CompareAttribute("UserPassword", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassowrd { get; set; }
    }
}
