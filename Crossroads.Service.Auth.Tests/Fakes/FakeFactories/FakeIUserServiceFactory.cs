using Crossroads.Service.Auth.Interfaces;
using Moq;

namespace Crossroads.Service.Auth.Tests.Fakes.FakeFactories
{
    public class FakeIUserServiceFactory : FakeFactory<IUserService>
    {
        public Mock<IUserService> FakeIUserService { get; }
        public FakeIUserServiceFactory(MockRepository mockRepository) : base(mockRepository)
        {
            FakeIUserService = mockRepository.Create<IUserService>();
        }

        public override IUserService Build()
        {
            return FakeIUserService.Object;
        }

        public void GetContactIdFromTokenReturnsInt(int contactId)
        {
            FakeIUserService.Setup(us => us.GetContactIdFromToken(
                It.IsAny<string>(), It.IsAny<CrossroadsDecodedToken>())).ReturnsAsync(contactId);            

        }
    }
}