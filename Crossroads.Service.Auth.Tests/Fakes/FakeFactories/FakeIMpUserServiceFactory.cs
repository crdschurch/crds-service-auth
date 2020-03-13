using System.Collections.Generic;
using Crossroads.Service.Auth.Interfaces;
using System.Threading.Tasks;
using Crossroads.Web.Auth.Models;
using Moq;

namespace Crossroads.Service.Auth.Tests.Fakes.FakeFactories
{
    public class FakeIMpUserServiceFactory : FakeFactory<IMpUserService>
    {
        public Mock<IMpUserService> FakeIMpUserService { get; }
        public FakeIMpUserServiceFactory(MockRepository mockRepository) : base(mockRepository)
        {
            FakeIMpUserService = mockRepository.Create<IMpUserService>();
        }        

        public override IMpUserService Build()
        {
            return FakeIMpUserService.Object;
        }

        public void GetMpUserInfoFromContactIdReturnsUserInfo(MpUserInfo userInfo)
        {
            FakeIMpUserService.Setup(us => us.GetMpUserInfoFromContactId(
            It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(userInfo);
        }

        public void GetMpUserInfoFromContactIdReturnsUserInfoSequence(MpUserInfo result1, MpUserInfo result2)
        {
            FakeIMpUserService.SetupSequence(us => us.GetMpUserInfoFromContactId(
            It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult<MpUserInfo>(result1))
            .Returns(Task.FromResult<MpUserInfo>(result2));
        }

        public void GetMpContactIdFromTokenReturnsInt(int contactId)
        {
            FakeIMpUserService.Setup(us => us.GetMpContactIdFromToken(
                It.IsAny<string>())).ReturnsAsync(contactId);        
        }
    }
}