using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TheradexPortal.Data.Services.Abstract;
//using Microsoft.Extensions.Logging.Log4Net.AspNetCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using TheradexPortal.Data.Models.Configuration;

namespace TheradexPortal.Data.Services
{
    public class OktaService : IOktaService
    {
        private readonly ILogger<OktaService> logger;
        private readonly HttpClient httpClient;

        public OktaService(HttpClient _httpClient, ILogger<OktaService> logger)
        {
            httpClient = _httpClient;
            this.logger = logger;
        }

        public async Task<Tuple<bool, string>> CreateUser(bool isProd, User curUser, bool isCTEP, bool isActive, string initialURL, string initialSite)
        {
            string loginType = "";
            string password = "";
            string oktaGroup = "";

            if (isCTEP)
            {
                password = GeneratePassword();
                loginType = "CTEP";
                oktaGroup = "THOR-NCI";
            }
            else
            {
                loginType = "Non-CTEP";
                oktaGroup = "THOR-Theradex";
            }

            try
            {
                logger.LogInformation("*** OKTA Call - Create User - " + curUser.EmailAddress + " (" + loginType + ") ***");

                OktaUser newOktaUser = new OktaUser();
                newOktaUser.profile = new OktaUser.Data();
                newOktaUser.profile.firstName = curUser.FirstName;
                newOktaUser.profile.lastName = curUser.LastName;
                newOktaUser.profile.email = curUser.EmailAddress;
                newOktaUser.profile.login = curUser.EmailAddress;
                newOktaUser.profile.initialSite = initialSite;
                newOktaUser.profile.initialURL = initialURL;
                newOktaUser.profile.loginType = loginType;
                newOktaUser.groupIds = new string[1];
                Tuple<bool, string> groupId = await FindGroup(oktaGroup);
                if (groupId.Item1)
                    newOktaUser.groupIds[0] = groupId.Item2;
                else
                {
                    logger.LogInformation("OKTA Fail: " + oktaGroup + " not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "Could not find " + oktaGroup + " in OKTA");
                }

                if (isCTEP)
                {
                    newOktaUser.credentials = new OktaUser.Credentials();
                    newOktaUser.credentials.password = new OktaUser.Credentials.Password();
                    newOktaUser.credentials.password.value = password;
                }

                string requestBody = JsonConvert.SerializeObject(newOktaUser);
                var requestBodyEncoded = new StringContent(requestBody, Encoding.UTF8, "application/json");                
                logger.LogInformation("OKTA Request Body: " + requestBody);

                HttpResponseMessage response = await httpClient.PostAsync("api/v1/users?activate=false", requestBodyEncoded);
                var contents = await response.Content.ReadAsStringAsync();
                logger.LogInformation("OKTA Response: " + contents);

                if (response.IsSuccessStatusCode)
                {
                    OktaUserDetail user = JsonConvert.DeserializeObject<OktaUserDetail>(contents);
                    string userId = user.id;
                    logger.LogInformation("OKTA - User creation successful");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(true, userId);
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.ReasonPhrase!);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UpdateUserInfo(string origEmail, User curUser)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Update User Details by Email Address - " + origEmail + " - " + curUser.EmailAddress + " ***");

                Tuple<bool, string> foundUser = await FindUser(origEmail);
                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    OktaUserUpdate newOktaUser = new OktaUserUpdate();
                    newOktaUser.profile = new OktaUserUpdate.Data();
                    newOktaUser.profile.firstName = curUser.FirstName;
                    newOktaUser.profile.lastName = curUser.LastName;
                    newOktaUser.profile.email = curUser.EmailAddress;
                    newOktaUser.profile.login = curUser.EmailAddress;
                    newOktaUser.profile.loginType = curUser.IsCtepUser ? "CTEP" : "Non-CTEP"; 

                    string requestBody = JsonConvert.SerializeObject(newOktaUser);
                    var requestBodyEncoded = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    logger.LogInformation("OKTA Request Body: " + requestBody);

                    HttpResponseMessage response = await httpClient.PostAsync("api/v1/users/" + userId, requestBodyEncoded);
                    var contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("OKTA - Update successful");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: User not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UpdateUserCTEP(string emailAddress, bool isCTEPUser)
        {
            // Change groups for user, remove then add - limited by API
            string userId = "";
            Tuple<bool, string> groupToRemove;
            Tuple<bool, string> groupToAdd;

            try
            {
                logger.LogInformation("*** OKTA Call - Update User CTEP/Non-CTEP group ***");

                logger.LogInformation("OKTA Get group id to Remove");
                if (isCTEPUser)
                {
                    groupToRemove = await FindGroup("THOR-Theradex");
                    groupToAdd = await FindGroup("THOR-NCI");
                }
                else
                {
                    groupToRemove = await FindGroup("THOR-NCI");
                    groupToAdd = await FindGroup("THOR-Theradex");
                }

                if (!groupToRemove.Item1 || !groupToAdd.Item1)
                {
                    logger.LogInformation("OKTA Fail: Could not find group to remove or add");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "Could not find group to remove or add");
                }

                Tuple<bool, string> foundUser = await FindUser(emailAddress);

                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    // Remove the user from  the group
                    logger.LogInformation("OKTA Remove user " + userId + " from group " + groupToRemove.Item2); 
                    HttpResponseMessage response = await httpClient.DeleteAsync("api/v1/groups/" + groupToRemove.Item2 + "/users/" + userId);
                    var contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase!);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }

                    logger.LogInformation("OKTA Add user " + userId + " to group " + groupToRemove.Item2);
                    response = await httpClient.PutAsync("api/v1/groups/" + groupToAdd.Item2 + "/users/" + userId, null);
                    contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("OKTA - Group switch successful");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: User not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UpdateActiveStatus(string userId, bool isActive, bool isCTEP)
        {
            HttpResponseMessage response;
            string contents;
            //string userId;

            logger.LogInformation("*** OKTA Call - Update User " + userId + " status to " + isActive.ToString() + " ***");
            try
            {
                if (isActive)
                {
                    response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/activate?sendEmail=false", null);
                    contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);
                }
                else
                {
                    response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/deactivate", null);
                    contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);
                }

                if (response.IsSuccessStatusCode)
                {
                    OktaActivation activation = JsonConvert.DeserializeObject<OktaActivation>(contents);
                    logger.LogInformation("OKTA URL: " + activation.activationUrl);
                    logger.LogInformation("OKTA Token: " + activation.activationToken);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(true, activation.activationUrl);
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.ReasonPhrase!);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> ReActivateUser(string emailAddress)
        {
            HttpResponseMessage response;
            string contents;
            string userId;

            logger.LogInformation("*** OKTA Call - Reactivate User Status - " + emailAddress + " ***");
            try
            {
                Tuple<bool, string> foundUser = await FindUser(emailAddress);
                if (foundUser != null)
                {
                    userId = foundUser.Item2;
                    response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/reactivate?sendEmail=false", null);
                    contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (response.IsSuccessStatusCode)
                    {
                        OktaActivation activation = JsonConvert.DeserializeObject<OktaActivation>(contents);
                        logger.LogInformation("OKTA URL: " + activation.activationUrl);
                        logger.LogInformation("OKTA Token: " + activation.activationToken);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, activation.activationUrl);
                    }
                    else
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: User not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }
        public async Task<Tuple<bool, string>> AddUserGroup(string emailAddress, bool isCTEPUser)
        {
            string userId = "";
            Tuple<bool, string> groupToAdd;

            try
            {
                logger.LogInformation("*** OKTA Call - Add User CTEP/Non-CTEP group - isCTEPUSer: " + isCTEPUser.ToString() + " ***");

                if (isCTEPUser)
                {
                    groupToAdd = await FindGroup("THOR-NCI");
                }
                else
                {
                    groupToAdd = await FindGroup("THOR-Theradex");
                }

                if (!groupToAdd.Item1)
                {
                    logger.LogInformation("OKTA Fail: Could not find group to remove or add");
                    return Tuple.Create(false, "Could not find group to remove or add");
                }

                Tuple<bool, string> foundUser = await FindUser(emailAddress);

                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;

                    logger.LogInformation("OKTA: Add user " + userId + " to group " + groupToAdd.Item2);
                    HttpResponseMessage response = await httpClient.PutAsync("api/v1/groups/" + groupToAdd.Item2 + "/users/" + userId, null);
                    var contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("OKTA - Add successful");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: User not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UnlockUser(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Unlock User by Email Address - " + emailAddress + " ***");

                Tuple<bool, string> foundUser = await FindUser(emailAddress);
                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    HttpResponseMessage response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/unlock", null);
                    var contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("OKTA Response: " + contents);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("OKTA - Unlock successful");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: User not found");
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> FindUser(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Find User by Email Address - " + emailAddress + " ***");
                HttpResponseMessage response = await httpClient.GetAsync("api/v1/users?search=profile.login eq \"" + WebUtility.UrlEncode(emailAddress) + "\"");
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("OKTA Response: " + contents);

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA User: " + emailAddress + " not found");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        dynamic user = JsonConvert.DeserializeObject<dynamic>(contents);
                        userId = user.First.id;
                        logger.LogInformation("OKTA User Id: " + userId);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, userId);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> GetUserStatus(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Get User Status by Email Address - " + emailAddress + " ***");

                HttpResponseMessage response = await httpClient.GetAsync("api/v1/users?search=profile.login eq \"" + WebUtility.UrlEncode(emailAddress) + "\"");
                var contents = await response.Content.ReadAsStringAsync();
                logger.LogInformation("OKTA Response: " + contents);

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA User: " + emailAddress + " not found");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        List<OktaUserDetail> user = JsonConvert.DeserializeObject<List<OktaUserDetail>>(contents);
                        logger.LogInformation("OKTA User Id: " + user[0]!.id + " - " + user[0]!.status);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, user[0]!.status);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> GetGroupList()
        {

            try
            {
                logger.LogInformation("*** OKTA Call - Get Groups ***");

                HttpResponseMessage response = await httpClient.GetAsync("api/v1/groups");
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("OKTA Response: " + contents);

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(true, "Call Successful");
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return (new Tuple<bool, string>(false, ex.Message));
            }
        }

        private async Task<Tuple<bool, string>> FindGroup(string groupName)
        {
            string groupId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Find Group - " + groupName + " ***");
                
                HttpResponseMessage response = await httpClient.GetAsync("api/v1/groups?q=" + groupName);
                var contents = await response.Content.ReadAsStringAsync();
                logger.LogInformation("OKTA Response: " + contents);

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA Group Id: " + groupName + " not found");
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        dynamic groupList = JsonConvert.DeserializeObject<dynamic>(contents);
                        groupId = groupList.First.id;
                        logger.LogInformation("OKTA Group Id: " + groupId);
                        logger.LogInformation("*** OKTA End ***");
                        return Tuple.Create(true, groupId);
                    }
                }
                else
                {
                    logger.LogInformation("OKTA Fail: " + response.ReasonPhrase);
                    logger.LogInformation("*** OKTA End ***");
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("OKTA Exception: " + ex.Message);
                logger.LogInformation("*** OKTA End ***");
                return Tuple.Create(false, ex.Message);
            }
        }

        private string GeneratePassword()
        {
            int length = 10;
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            string password = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string charsCaps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string charsLower = "abcdefghijklmnopqrstuvwxyz";
            string charsNumber = "0123456789";
            string charsSpecial = "!@#$%^&*()_+";

            // Make sure at least 1 from each category is in the new password
            string pw1 = new string(Enumerable.Repeat(charsCaps, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string pw2 = new string(Enumerable.Repeat(charsLower, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string pw3 = new string(Enumerable.Repeat(charsNumber, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string pw4 = new string(Enumerable.Repeat(charsSpecial, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password + pw1 + pw2 + pw3 + pw4;
        }
    }
    public class OktaUser
    {
        public Data profile;
        public class Data
        {
            public string firstName = "";
            public string lastName = "";
            public string email = "";
            public string login = "";
            public string initialSite = "";
            public string initialURL = "";
            public string loginType = "";
        }
        public string[] groupIds;
        public Credentials credentials;
        public class Credentials
        {
            public Password password;
            public class Password
            {
                public string value = "";
            }
        }
    }

    public class OktaUserUpdate
    {
        public Data profile;
        public class Data
        {
            public string firstName = "";
            public string lastName = "";
            public string email = "";
            public string login = "";
            public string loginType = "";
        }
    }

    public class OktaUserDetail
    { 
        public string id = "";
        public string status = "";
    }

    public class OktaActivation
    {
        public string activationUrl = "";
        public string activationToken = "";
    }
}
