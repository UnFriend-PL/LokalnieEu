using Newtonsoft.Json;

namespace LokalnieEU.Models.User
{
     public class UpdateUserPasswordDto
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; } = string.Empty;
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; } = string.Empty;

    }
}
