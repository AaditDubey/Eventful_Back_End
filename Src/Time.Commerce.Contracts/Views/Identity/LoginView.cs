using System.Text.Json.Serialization;

namespace Time.Commerce.Contracts.Views.Identity
{
    public class LoginView
    {
        public UserView User { get; set; }
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }
        public decimal Exp { get; set; }
    }
}
