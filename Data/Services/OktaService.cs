using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;
using Newtonsoft.Json;

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

        public async Task<Tuple<bool, string>> CreateUser(bool isProd, string firstName, string lastName, string emailAddress, bool isCTEP, bool isActive, string initialURL, string initialSite)
        {
            // Add an OKTA user with the fields passed in plus:
            //  initialSite
            //  loginType (CTEP or Non-CTEP)
            //  initialURL

            //  If CTEP user, Activate the user, define a password
            //  If Non-CTEP user, do not activate or define a password;

            // For non-prod environments, we may already have an OKTA user already (OKTA Beta includes our non-prod environments) - switch to new method to update what is needed
            if (!isProd)
            {
                // Determine if we have an OKTA user
                Tuple<bool, string> user =  await FindUser(emailAddress);
                // If we find one for a non-prod environment, there is nothing to do.
                if (user.Item1 == true)
                {
                    return Tuple.Create(true, emailAddress + ": User exists - processing ended.");
                }
                else if (user.Item1 == false && user.Item2 != "")
                {
                    return Tuple.Create(false, "Failed trying to find user");
                }
            }

            string loginType = "";
            string password = "";
            string oktaGroup = "";


            if (isCTEP)
            {
                password = GeneratePassword();
                loginType = "CTEP";
                oktaGroup = "Web Reporting-NCI";
            }
            else
            {
                loginType = "Non-CTEP";
                oktaGroup = "Web Reporting-Theradex";
            }

            try
            {
                logger.LogInformation("*** OKTA Call - Create User (" + loginType + ") ***");

                OktaUser newOktaUser = new OktaUser();
                newOktaUser.profile = new OktaUser.Data();
                newOktaUser.profile.firstName = firstName;
                newOktaUser.profile.lastName = lastName;
                newOktaUser.profile.email = emailAddress;
                newOktaUser.profile.login = emailAddress;
                newOktaUser.profile.initialSite = initialSite;
                newOktaUser.profile.initialURL = initialURL;
                newOktaUser.profile.loginType = loginType;
                newOktaUser.groupIds = new string[1];
                Tuple<bool, string> groupId = await FindGroup(oktaGroup);
                if (groupId.Item1)
                    newOktaUser.groupIds[0] = groupId.Item2;
                else
                    return Tuple.Create(false, "Could not find " + oktaGroup + " in OKTA");

                if (isCTEP)
                {
                    newOktaUser.credentials = new OktaUser.Credentials();
                    newOktaUser.credentials.password = new OktaUser.Credentials.Password();
                    newOktaUser.credentials.password.value = password;
                }

                string requestBody = JsonConvert.SerializeObject(newOktaUser);
                var requestBodyEncoded = new StringContent(requestBody, Encoding.UTF8, "application/json");
                
                logger.LogInformation("Request Body:");
                logger.LogInformation(requestBody);

                HttpResponseMessage response = await httpClient.PostAsync("api/v1/users?activate=false", requestBodyEncoded);
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("Response: ");
                logger.LogInformation(contents);
                logger.LogInformation("-------------------------------------------------");

                if (response.IsSuccessStatusCode)
                {
                    // Activate in OKTA.
                    Tuple<bool, string> activateResult = await UpdateActiveStatus(emailAddress, true, isCTEP);                    
                }

                if (response.IsSuccessStatusCode)
                {
                    return Tuple.Create(true, "Call Successful");
                }
                else
                {
                    return Tuple.Create(false, response.ReasonPhrase!);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UpdateUserInfo(string origEmail, string firstName, string lastName, string emailAddress, bool isCTEPUser)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Update User Details by Email Address ***");

                Tuple<bool, string> foundUser = await FindUser(origEmail);
                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    OktaUserUpdate newOktaUser = new OktaUserUpdate();
                    newOktaUser.profile = new OktaUserUpdate.Data();
                    newOktaUser.profile.firstName = firstName;
                    newOktaUser.profile.lastName = lastName;
                    newOktaUser.profile.email = emailAddress;
                    newOktaUser.profile.login = emailAddress;
                    newOktaUser.profile.loginType = isCTEPUser ? "CTEP" : "Non-CTEP"; 

                    string requestBody = JsonConvert.SerializeObject(newOktaUser);
                    var requestBodyEncoded = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    logger.LogInformation("Request Body:");
                    logger.LogInformation(requestBody);

                    HttpResponseMessage response = await httpClient.PostAsync("api/v1/users/" + userId, requestBodyEncoded);
                    var contents = await response.Content.ReadAsStringAsync();

                    logger.LogInformation("Response: ");
                    logger.LogInformation(contents);
                    logger.LogInformation("-------------------------------------------------");

                    if (response.IsSuccessStatusCode)
                    {
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                    return Tuple.Create(false, "User not found");
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
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

                logger.LogInformation("Get OKTA group id to Remove");
                if (isCTEPUser)
                {
                    groupToRemove = await FindGroup("Web Reporting-Theradex");
                    groupToAdd = await FindGroup("Web Reporting-NCI");
                }
                else
                {
                    groupToRemove = await FindGroup("Web Reporting-NCI");
                    groupToAdd = await FindGroup("Web Reporting-Theradex");
                }

                if (!groupToRemove.Item1 || !groupToAdd.Item1)
                {
                    return Tuple.Create(false, "Could not find group to remove or add");
                }

                Tuple<bool, string> foundUser = await FindUser(emailAddress);

                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    // Remove the user from  the group
                    logger.LogInformation("Remove user " + userId + " from group " + groupToRemove.Item2); 
                    HttpResponseMessage response = await httpClient.DeleteAsync("api/v1/groups/" + groupToRemove.Item2 + "/users/" + userId);
                    var contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("Response: ");
                    logger.LogInformation(contents);

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }

                    logger.LogInformation("Add user " + userId + " to group " + groupToRemove.Item2);
                    response = await httpClient.PutAsync("api/v1/groups/" + groupToAdd.Item2 + "/users/" + userId, null);
                    contents = await response.Content.ReadAsStringAsync();
                    logger.LogInformation("Response: ");
                    logger.LogInformation(contents);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("-------------------------------------------------");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UpdateActiveStatus(string emailAddress, bool isActive, bool isCTEP)
        {
            HttpResponseMessage response;
            string contents;
            string userId;

            logger.LogInformation("*** OKTA Call - Update User Active Status to " + isActive.ToString() + " ***");
            try
            {
                Tuple<bool, string> foundUser = await FindUser(emailAddress);
                if (foundUser != null)
                {
                    userId = foundUser.Item2;
                    if (isActive)
                    {
                        response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/activate?sendEmail=false", null);
                        contents = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/deactivate", null);
                        contents = await response.Content.ReadAsStringAsync();
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                {
                    logger.LogInformation("-------------------------------------------------");
                    return Tuple.Create(false, "User not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> UnlockUser(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Unlock User by Email Address ***");

                Tuple<bool, string> foundUser = await FindUser(emailAddress);
                if (foundUser.Item1)
                {
                    userId = foundUser.Item2;
                    HttpResponseMessage response = await httpClient.PostAsync("api/v1/users/" + userId + "/lifecycle/unlock", null);
                    var contents = await response.Content.ReadAsStringAsync();

                    logger.LogInformation("Response: ");
                    logger.LogInformation(contents);
                    logger.LogInformation("-------------------------------------------------");

                    if (response.IsSuccessStatusCode)
                    {
                        return Tuple.Create(true, "Call Successful");
                    }
                    else
                    {
                        return Tuple.Create(false, response.ReasonPhrase!);
                    }
                }
                else
                    return Tuple.Create(false, "User not found");
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> FindUser(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Find User by Email Address ***");
                
                HttpResponseMessage response = await httpClient.GetAsync("api/v1/users?search=profile.login eq \"" + WebUtility.UrlEncode(emailAddress) + "\"");
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("Response: ");
                logger.LogInformation(contents);

                logger.LogInformation("-------------------------------------------------");

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA User: " + emailAddress + " not found");
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        dynamic user = JsonConvert.DeserializeObject<dynamic>(contents);
                        userId = user.First.id;
                        logger.LogInformation("OKTA User Id: " + userId);
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(true, userId);
                    }
                }
                else
                {
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> GetUserStatus(string emailAddress)
        {
            string userId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Get User Details by Email Address ***");

                HttpResponseMessage response = await httpClient.GetAsync("api/v1/users?search=profile.login eq \"" + WebUtility.UrlEncode(emailAddress) + "\"");
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("Response: ");
                logger.LogInformation(contents);

                logger.LogInformation("-------------------------------------------------");

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA User: " + emailAddress + " not found");
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        List<OktaUserDetail> user = JsonConvert.DeserializeObject<List<OktaUserDetail>>(contents);
                        logger.LogInformation("OKTA User Id: " + user[0]!.id);
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(true, user[0]!.status);
                    }
                }
                else
                {
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
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

                logger.LogInformation("Response: ");
                logger.LogInformation(contents);
                logger.LogInformation("-------------------------------------------------");

                if (response.IsSuccessStatusCode)
                {
                    return Tuple.Create(true, "Call Successful");
                }
                else
                {
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                // Log the message in the ErrorLog
                logger.LogInformation("-------------------------------------------------");
                return (new Tuple<bool, string>(false, ex.Message));
            }
        }

        private async Task<Tuple<bool, string>> FindGroup(string groupName)
        {
            string groupId = "";
            try
            {
                logger.LogInformation("*** OKTA Call - Find Group ***");
                
                HttpResponseMessage response = await httpClient.GetAsync("api/v1/groups?q=" + groupName);
                var contents = await response.Content.ReadAsStringAsync();

                logger.LogInformation("Response: ");
                logger.LogInformation(contents);

                if (response.IsSuccessStatusCode)
                {
                    if (contents == "[]")
                    {
                        logger.LogInformation("OKTA Group Id: " + groupName + " not found");
                        logger.LogInformation("-------------------------------------------------");
                        return Tuple.Create(false, "");
                    }
                    else
                    {
                        dynamic groupList = JsonConvert.DeserializeObject<dynamic>(contents);
                        groupId = groupList.First.id;
                        return Tuple.Create(true, groupId);
                        logger.LogInformation("OKTA Group Id: " + groupId);
                        logger.LogInformation("-------------------------------------------------");
                    }
                }
                else
                {
                    return Tuple.Create(false, response.StatusCode + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("-------------------------------------------------");
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
}
