using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.Models
{
    public class AccessToken
    {
        [JsonProperty("access_token_6484424")]
        public string Token { get; set; }
        [JsonProperty("expires_in")]
        public int Expires { get; set; }

        public bool IsEmpty => string.Empty.Equals(Token);
    }
}
