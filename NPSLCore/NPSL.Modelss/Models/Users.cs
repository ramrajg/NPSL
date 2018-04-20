namespace NPSLCore.Models.DB
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public string Company { get; set; }
        public string LoginId { get; set; }
        public string LoginPassword { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
    }
}
