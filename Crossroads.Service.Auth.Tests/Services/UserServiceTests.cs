using System.Collections;
using Xunit;
using Moq;
using Crossroads.Web.Auth.Models;
using System.IdentityModel.Tokens.Jwt;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Identity.Tests.Fakes.IsolatedFactories;

namespace Crossroads.Service.Auth.Tests
{
    public class UserServiceTests
    {        
        private IsolatedUserServiceFactory createUserServiceFactory()
        {
            var mockRepository = new MockRepository(MockBehavior.Loose);
            var serviceFactory = new IsolatedUserServiceFactory(mockRepository);
            return serviceFactory;
        }

        [Theory]
        [InlineData("mp")]
        [InlineData("okta")]
        public void GetUserInfo_WithValidToken_ReturnValidUserInfo(string provider)
        {
            //Arrange
            var serviceFactory = createUserServiceFactory();   
            var originalToken = "abcdefg";
            var crdsDecodedToken = new CrossroadsDecodedToken(){
                decodedToken = new JwtSecurityToken() {},
                authProvider = provider
            };
            var mpAPIToken = "apiapi";
            var contactId = 12345;
            var userInfo = new MpUserInfo(){
                ContactId = contactId
            };         

            serviceFactory.OktaUserService.GetMpContactIdFromDecodedTokenReturnsInt(contactId);                        
            serviceFactory.MpUserService.GetMpUserInfoFromContactIdReturnsUserInfo(userInfo);
            var userService = serviceFactory.Build();
            
            //Act
            var result = userService.GetUserInfo(originalToken, crdsDecodedToken, mpAPIToken).Result;            

            //Assert
            Assert.Equal(result.Mp.ContactId, contactId);
        }

        /*[Fact]
        public void GetUserInfo_WithInitialInvalidContactId_RetrievesValidContactIdAndReturnsValidUserInfo()
        {
            //Arrange
            var serviceFactory = createUserServiceFactory();   
            var originalToken = "abcdefg";
            IEnumerable<JsonClaimValueTypes> claims = new JsonClaimValueTypes(){
                "uid" = 12345
            }
            var crdsDecodedToken = new CrossroadsDecodedToken(){
                decodedToken = new JwtSecurityToken() {
                    Payload = new JwtPayload()
                },
                authProvider = "mp"
            };
            var mpAPIToken = "apiapi";
            var contactId = 12345;
            var userInfo = new MpUserInfo(){
                ContactId = contactId
            };         

            serviceFactory.OktaUserService.GetMpContactIdFromDecodedTokenReturnsInt(contactId);                        
            serviceFactory.MpUserService.GetMpUserInfoFromContactIdReturnsUserInfoSequence(null, userInfo);
            var userService = serviceFactory.Build();
            
            //Act
            var result = userService.GetUserInfo(originalToken, crdsDecodedToken, mpAPIToken).Result;            

            //Assert
            Assert.Equal(result.Mp.ContactId, contactId);
        }*/
    }
}
