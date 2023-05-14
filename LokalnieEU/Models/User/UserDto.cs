namespace LokalnieEU.Models.User
{
    public class UserDto
    {
        public required string Name { get; set; } = string.Empty;
        public required string Surname { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Phone { get; set; } = string.Empty;
    }
}
