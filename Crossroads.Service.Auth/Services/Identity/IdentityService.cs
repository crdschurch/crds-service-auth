using System.Threading.Tasks;
using System.Net.Http;
using Crossroads.Microservice.Settings;
using Newtonsoft.Json;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Auth.Models;

namespace Crossroads.Service.Auth.Services
{
    public class IdentityService : IIdentityservice
    {
        protected virtual HttpClient client => _client;
        private static readonly HttpClient _client = new HttpClient();
        private string _identityApiUrl;
        private string _identitySharedSecret;

        public IdentityService(ISettingsService settingsService)
        {
            _identityApiUrl = settingsService.GetSetting("IDENTITY_SERVICE_URL");
            _identitySharedSecret = settingsService.GetSetting("IDENTITY_SHARED_SECRET");
        }

        public async Task<int> GetValidContactIdFromIdentity(string oktaId, int invalidMPContactId)
        {
            int newContactId = -1;
            //send request
            var response = await GetUpdatedContactId(oktaId, invalidMPContactId);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<IdentityContact>(data);
                newContactId = contact.ContactId;
            }
            return newContactId;
        }

        private async Task<HttpResponseMessage> GetUpdatedContactId(string oktaId, int invalidMPContactId)
        {
            //create request
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_identityApiUrl}/api/identities/{invalidMPContactId}/updatedcontact" );
            //send request            
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("OktaID", oktaId);            
            request.Headers.Add("Authorization", _identitySharedSecret);
            return await client.SendAsync(request);
        }
    }
}