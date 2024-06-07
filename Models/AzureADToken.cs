using System.Text.Json.Serialization;

namespace VERAExample.Models
{
    public class AzureADToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonPropertyName("ext_expires_in")]
        public string ExtExpiresIn
        {
            get; set;
        }

        [JsonPropertyName("expires_on")]
        public string ExpiresOn { get; set; }

        [JsonPropertyName("not_before")]
        public string NotBefore { get; set; }

        public string Resource { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
