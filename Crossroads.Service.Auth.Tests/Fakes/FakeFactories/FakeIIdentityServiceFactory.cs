using System.Collections.Generic;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Auth.Tests.Fakes;
using Moq;

namespace Crossroads.Service.Auth.Tests.Fakes.FakeFactories
{
    public class FakeIIdentityServiceFactory : FakeFactory<IIdentityService>
    {
        public Mock<IIdentityService> FakeIIdentityService { get; }
        public FakeIIdentityServiceFactory(MockRepository mockRepository) : base(mockRepository)
        {
            FakeIIdentityService = mockRepository.Create<IIdentityService>();
        }        

        public override IIdentityService Build()
        {
            return FakeIIdentityService.Object;
        }

        public void GetValidContactIdFromIdentityReturnsInt(int newContactId)
        {
            FakeIIdentityService.Setup(id => id.GetValidContactIdFromIdentity(
                It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(newContactId);
        }
    }
}