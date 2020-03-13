using Crossroads.Service.Auth.Services;
using Crossroads.Service.Auth.Interfaces;
using NLog;

namespace Crossroads.Service.Auth.Tests.Fakes.IsolatedClasses
{
    public class IsolatedUserService : UserService
    {
        private IMpUserService mpUserService;
        private IOktaUserService oktaUserService;
        private IIdentityService identityService;        

        public IsolatedUserService(
            IMpUserService fakeMpUserService, IOktaUserService fakeOktaUserService, IIdentityService fakeIdentityService) 
            : base(fakeMpUserService, fakeOktaUserService, fakeIdentityService)
        {
            mpUserService = fakeMpUserService;
            oktaUserService = fakeOktaUserService;
            identityService = fakeIdentityService;
        }
    }
}