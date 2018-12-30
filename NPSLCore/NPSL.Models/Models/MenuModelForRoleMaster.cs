using System.Collections.Generic;

namespace NPSLCore.Models.DB
{
    public partial class MenuModelForRoleMaster
    {
        public string MainMenuName { get; set; }
        public int MainMenuId { get; set; }
        public string SubMenuName { get; set; }
        public int SubMenuId { get; set; }
        public int IsSelected { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
       
    }
}
