using System.Threading.Tasks;
using Crossroads.Web.Auth.Models;

namespace Crossroads.Service.Auth.Interfaces
{
    public interface IIdentityservice
    {
        Task<int> GetValidContactIdFromIdentity(string oktaId, int invalidMPContactId);
    }
}