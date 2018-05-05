namespace NPSLCore.Models.DB
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string UserType { get; set; }
    }
}
