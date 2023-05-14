using System.ComponentModel.DataAnnotations;

namespace LokalnieEU.Models.User
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime LastLoginTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
    public class RoleEmails
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
