using System.Collections.Generic;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Auth.Tests.Fakes;
using Moq;

namespace Crossroads.Service.Auth.Tests.Fakes.FakeFactories
{
    public class FakeIOktaUserServiceFactory : FakeFactory<IOktaUserService>
    {
        public Mock<IOktaUserService> FakeIOktaUserService { get; }
        public FakeIOktaUserServiceFactory(MockRepository mockRepository) : base(mockRepository)
        {
            FakeIOktaUserService = mockRepository.Create<IOktaUserService>();
        }        

        public override IOktaUserService Build()
        {
            return FakeIOktaUserService.Object;
        }

        public void GetMpContactIdFromDecodedTokenReturnsInt(int contactId)
        {
            FakeIOktaUserService.Setup(us => us.GetMpContactIdFromDecodedToken(
                It.IsAny<CrossroadsDecodedToken>())).Returns(contactId);
        }
    }
}