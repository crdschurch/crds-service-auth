using System.Threading.Tasks;

namespace Crossroads.Service.Auth.Interfaces
{
    public interface IIdentityService
    {
        Task<int> GetValidContactIdFromIdentity(string oktaId, int invalidMPContactId);
    }
}