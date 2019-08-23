using Crossroads.Web.Auth.Models;
using System.Threading.Tasks;

namespace Crossroads.Service.Auth.Interfaces
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfo(string originalToken, CrossroadsDecodedToken crossroadsDecodedToken, string mpAPIToken);

        Task<Authorization> GetAuthorizations(CrossroadsDecodedToken crossroadsDecodedToken, string mpAPIToken, int mpContactId);
    }
}
