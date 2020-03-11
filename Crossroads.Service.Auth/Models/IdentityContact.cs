using Newtonsoft.Json;

namespace Crossroads.Service.Auth.Models
{
    public class IdentityContact
    {
        [JsonProperty(PropertyName = "updatedContactId")]
        public int ContactId { get; set; }

    }
}