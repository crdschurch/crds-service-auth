﻿using System.Collections.Generic;
using Crossroads.Service.Auth.Exceptions;
using Crossroads.Web.Auth.Models;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Models;
using Newtonsoft.Json.Linq;
using Crossroads.Service.Auth.Interfaces;
using System.Threading.Tasks;

namespace Crossroads.Service.Auth.Services
{
    public class MpUserService : IMpUserService
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private IAuthenticationRepository _authenticationRepository;
        private IMinistryPlatformRestRequestBuilderFactory _mpRestBuilder;

        public MpUserService(IAuthenticationRepository authenticationRepository,
                             IMinistryPlatformRestRequestBuilderFactory mpRestBuilder)
        {
            _authenticationRepository = authenticationRepository;
            _mpRestBuilder = mpRestBuilder;
        }

        public async Task<int> GetMpContactIdFromToken(string token)
        {
            int contactId = await _authenticationRepository.GetContactIdAsync(token);

            return contactId;
        }

        public async Task<MpUserInfo> GetMpUserInfoFromContactId(int contactId, string mpAPIToken)
        {
            if (contactId > 0)
            {
                return await GetMpUserInfo(contactId, mpAPIToken);
            }
            else
            {
                _logger.Error("No contactId Available for token");
                throw new NoContactIdAvailableException();
            }
        }

        public async Task<Dictionary<int, string>> GetRoles(string mpAPIToken, int mpContactId)
        {
            Dictionary<int, string> rolesDict = new Dictionary<int, string>();

            // Go get the roles from mp
            var columns = new string[] {
                    "dp_User_Roles.Role_ID",
                    "Role_ID_Table.Role_Name"
                };

            var roles = await _mpRestBuilder.NewRequestBuilder()
                                      .WithAuthenticationToken(mpAPIToken)
                                      .WithSelectColumns(columns)
                                      .WithFilter($"User_ID_Table_Contact_ID_Table.[Contact_ID]={mpContactId}")
                                      .BuildAsync()
                                      .Search<JObject>("dp_User_Roles");

            if (roles == null)
            {
                return rolesDict;
            }

            foreach (var role in roles)
            {
                rolesDict.Add(role.Value<int>("Role_ID"), role.Value<string>("Role_Name"));
            }

            return rolesDict;
        }

        private async Task<MpUserInfo> GetMpUserInfo(int contactId, string mpAPIToken)
        {
            var columns = new string[] {
                    "Contacts.Contact_ID",
                    "User_Account",
                    "Donor_Record",
                    "Participant_Record",
                    "Email_Address",
                    "Household_ID",
                    "User_Account_Table.Can_Impersonate"
                };

            var result = await _mpRestBuilder.NewRequestBuilder()
                                        .WithAuthenticationToken(mpAPIToken)
                                        .WithSelectColumns(columns)
                                        .WithFilter("Contacts.Contact_ID=" + contactId.ToString())
                                        .BuildAsync()
                                        .Search<MpContact>();
            
            if (result.Count != 1) {
                string errorString = "Invalid result length for mp user info query. Num Results: " + result.Count.ToString();
                _logger.Error(errorString);
                throw new InvalidNumberOfResultsForMpContact(errorString);
            }

            var contact = result[0];

            MpUserInfo mpUserInfoDTO = new MpUserInfo
            {
                ContactId = contactId,
                UserId = contact.UserAccount,
                ParticipantId = contact.ParticipantRecord,
                HouseholdId = contact.HouseholdId,
                Email = contact.EmailAddress,
                DonorId = contact.DonorRecord,
                CanImpersonate = contact.CanImpersonate ?? false
            };

            return mpUserInfoDTO;
        }
    }
}
