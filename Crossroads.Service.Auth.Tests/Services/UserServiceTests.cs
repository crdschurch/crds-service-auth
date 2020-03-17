using System.Collections;
using Xunit;
using Moq;
using System.Security.Claims;
using Crossroads.Web.Auth.Models;
using System.IdentityModel.Tokens.Jwt;
using Crossroads.Service.Auth.Interfaces;
using Crossroads.Service.Identity.Tests.Fakes.IsolatedFactories;
using Crossroads.Service.Auth.Exceptions;

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

        [Theory]
        [InlineData("mp")]
        [InlineData("okta")]
        public void GetUserInfo_WithInitialInvalidContactId_RetrievesValidContactIdAndReturnsValidUserInfo(string provider)
        {
            //Arrange
            var serviceFactory = createUserServiceFactory();   
            var originalToken = "abcdefg";
            JwtHeader header = new JwtHeader();
            JwtPayload payload = new JwtPayload();            
            payload.AddClaim(new Claim("uid", "12345"));
            var crdsDecodedToken = new CrossroadsDecodedToken(){
                decodedToken = new JwtSecurityToken(header, payload){
                },
                authProvider = provider
            };
            var mpAPIToken = "apiapi";
            var contactId = 12345;
            var userInfo = new MpUserInfo(){
                ContactId = contactId
            };
            serviceFactory.IdentityService.GetValidContactIdFromIdentityReturnsInt(contactId);                     
            serviceFactory.OktaUserService.GetMpContactIdFromDecodedTokenReturnsInt(contactId);                        
            serviceFactory.MpUserService.GetMpUserInfoFromContactIdReturnsUserInfoSequence(null, userInfo);
            var userService = serviceFactory.Build();
            
            //Act
            var result = userService.GetUserInfo(originalToken, crdsDecodedToken, mpAPIToken).Result;            

            //Assert
            Assert.Equal(result.Mp.ContactId, contactId);
        }

        [Theory]
        [InlineData("mp")]
        [InlineData("okta")]
        public async void GetUserInfo_WhenIdentityServiceReturnsInvalidContactId_ReturnException(string provider)
        {
            //Arrange
            var serviceFactory = createUserServiceFactory();   
            var originalToken = "abcdefg";
            JwtHeader header = new JwtHeader();
            JwtPayload payload = new JwtPayload();            
            payload.AddClaim(new Claim("uid", "12345"));
            var crdsDecodedToken = new CrossroadsDecodedToken(){
                decodedToken = new JwtSecurityToken(header, payload){
                },
                authProvider = provider
            };
            var mpAPIToken = "apiapi";
            var contactId = 12345;
            var userInfo = new MpUserInfo(){
                ContactId = contactId
            };         

            serviceFactory.IdentityService.GetValidContactIdFromIdentityReturnsInt(-1);
            serviceFactory.OktaUserService.GetMpContactIdFromDecodedTokenReturnsInt(contactId);                        
            serviceFactory.MpUserService.GetMpUserInfoFromContactIdReturnsUserInfoSequence(null, userInfo);
            var userService = serviceFactory.Build();
            
            //Act / Assert
            await Assert.ThrowsAsync<InvalidNumberOfResultsForMpContact>(() => userService.GetUserInfo(originalToken, crdsDecodedToken, mpAPIToken));
        }
    }
}
