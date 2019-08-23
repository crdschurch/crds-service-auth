using Crossroads.Service.Auth.Constants;
using Crossroads.Web.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Auth.Exceptions;
using System.Threading.Tasks;

namespace Crossroads.Service.Auth.Services
{
    public class UserService : IUserService
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private IMpUserService _mpUserService;
        private IOktaUserService _oktaUserService;

        public UserService(IMpUserService mpUserService, IOktaUserService oktaUserService)
        {
            _mpUserService = mpUserService;
            _oktaUserService = oktaUserService;
        }

        public async Task<UserInfo> GetUserInfo(string originalToken,
                                       CrossroadsDecodedToken crossroadsDecodedToken,
                                       string mpAPIToken)
        {
            UserInfo userInfoObject = new UserInfo();
            int contactId = await GetContactIdFromToken(originalToken, crossroadsDecodedToken);

            userInfoObject.Mp = await _mpUserService.GetMpUserInfoFromContactId(contactId, mpAPIToken);

            return userInfoObject;
        }

        public async Task<Authorization> GetAuthorizations(CrossroadsDecodedToken crossroadsDecodedToken, string mpAPIToken, int mpContactId)
        {
            Authorization authorizationObject = new Authorization();

            if (crossroadsDecodedToken.authProvider == AuthConstants.AUTH_PROVIDER_OKTA)
            {
                authorizationObject.OktaRoles = _oktaUserService.GetRoles(crossroadsDecodedToken);
            }

            authorizationObject.MpRoles = await _mpUserService.GetRoles(mpAPIToken, mpContactId);

            return authorizationObject;
        }

        private async Task<int> GetContactIdFromToken(string originalToken, CrossroadsDecodedToken crossroadsDecodedToken)
        {
            int contactId = -1;

            if (crossroadsDecodedToken.authProvider == AuthConstants.AUTH_PROVIDER_OKTA)
            {
                contactId = _oktaUserService.GetMpContactIdFromDecodedToken(crossroadsDecodedToken);
            }
            else if (crossroadsDecodedToken.authProvider == AuthConstants.AUTH_PROVIDER_MP)
            {
                contactId = await _mpUserService.GetMpContactIdFromToken(originalToken);
            }
            else
            {
                //This should never happen based on previous logic
                _logger.Warn("Invalid issuer when there should not be an invalid issuer w/ token: " + originalToken);
                throw new SecurityTokenInvalidIssuerException();
            }

            if (contactId == -1)
            {
                string exceptionString = $"No mpContactID available for JWT with issuer: {crossroadsDecodedToken.authProvider}, and JWT id: {crossroadsDecodedToken.decodedToken.Id}";
                _logger.Error(exceptionString);
                throw new NoContactIdAvailableException(exceptionString);
            }

            return contactId;
        }
    }
}
