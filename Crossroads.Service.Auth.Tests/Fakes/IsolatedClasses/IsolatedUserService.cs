using Crossroads.Service.Auth.Services;
using Crossroads.Service.Auth.Interfaces;
using NLog;

namespace Crossroads.Service.Auth.Tests.Fakes.IsolatedClasses
{
    public class IsolatedUserService : UserService
    {  
        public IsolatedUserService(
            IMpUserService fakeMpUserService, IOktaUserService fakeOktaUserService, IIdentityService fakeIdentityService) 
            : base(fakeMpUserService, fakeOktaUserService, fakeIdentityService)
        {
            
        }
    }
}