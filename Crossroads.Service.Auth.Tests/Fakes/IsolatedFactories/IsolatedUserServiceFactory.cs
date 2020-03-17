using Crossroads.Service.Auth.Services;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Auth.Tests.Fakes;
using Crossroads.Service.Auth.Tests.Fakes.FakeFactories;
using Crossroads.Service.Auth.Tests.Fakes.IsolatedClasses;
using Moq;
using NLog;

namespace Crossroads.Service.Identity.Tests.Fakes.IsolatedFactories
{
    public class IsolatedUserServiceFactory : FakeFactory<UserService>
    {        
        public FakeIMpUserServiceFactory MpUserService { get; }
        public FakeIOktaUserServiceFactory OktaUserService { get; }
        public FakeIIdentityServiceFactory IdentityService { get; }        
        
        public IsolatedUserServiceFactory(MockRepository mockRepository) : base(mockRepository)
        {
            MpUserService = new FakeIMpUserServiceFactory(mockRepository);
            IdentityService = new FakeIIdentityServiceFactory(mockRepository);
            OktaUserService = new FakeIOktaUserServiceFactory(mockRepository);            
        }

        public override UserService Build()
        {
            IMpUserService fakeMpUserService = MpUserService.Build();
            IOktaUserService fakeOktaUserService = OktaUserService.Build();
            IIdentityService fakeIdentityService = IdentityService.Build();            
            return new IsolatedUserService(fakeMpUserService, fakeOktaUserService, fakeIdentityService);
        }
    }
}