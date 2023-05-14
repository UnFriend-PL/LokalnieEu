using Newtonsoft.Json;

namespace LokalnieEU.Models.User
{
    public class UserResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("surname")]
        public string Surname { get; set; } = string.Empty;
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public UserResponse()
        {

        }
        public UserResponse(User user, string token)
        {
            UserId = user.UserId; 
            Name = user.Name; 
            Surname = user.Surname; 
            Email = user.Email; 
            Phone = user.Phone;
            Token = token;
        }
    }

}
