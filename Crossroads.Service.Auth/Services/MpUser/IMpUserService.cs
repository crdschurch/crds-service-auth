using System.Collections.Generic;
using System.Threading.Tasks;
using Crossroads.Web.Auth.Models;

namespace Crossroads.Service.Auth.Interfaces
{
    public interface IMpUserService
    {
        Task<int> GetMpContactIdFromToken(string token);

        Task<MpUserInfo> GetMpUserInfoFromContactId(int contactId, string mpAPIToken);

        Task<Dictionary<int, string>> GetRoles(string mpAPIToken, int mpContactId);
    }
}
