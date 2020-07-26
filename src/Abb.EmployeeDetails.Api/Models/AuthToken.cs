using Newtonsoft.Json;

namespace Abb.EmployeeDetails.Api.Models
{
    public class AuthToken
    {
        [JsonIgnore]
        public string EndPoint { get; set; }

        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty(PropertyName = "audience")]
        public string audience { get; set; }

        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; }

    }
}
