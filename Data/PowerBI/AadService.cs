namespace TheradexPortal.Data.PowerBI
{
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Client;
    using System;
    using System.Linq;
    using System.Security;
    using System.Text.Json;
    using TheradexPortal.Data.PowerBI.Abstract;
    using TheradexPortal.Data.PowerBI.Models;

    public class AadService : IAadService
    {
        private readonly IOptions<AzureAd> azureAd;

        public AadService(IOptions<AzureAd> azureAd)
        {
            this.azureAd = azureAd;
        }

        /// <summary>
        /// Generates and returns Access token
        /// </summary>
        /// <returns>AAD token</returns>
        public string GetAccessToken()
        {
            if (azureAd == null)
            {
                throw new ArgumentNullException("Missing PowerBI credentials");
            }

            AuthenticationResult? authenticationResult = null;
            if (azureAd.Value.AuthenticationMode.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                // Create a public client to authorize the app with the AAD app
                IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(azureAd.Value.ClientId).WithAuthority(azureAd.Value.AuthorityUri).Build();
                var userAccounts = clientApp.GetAccountsAsync().Result;
                // Retrieve Access token from cache if available
                authenticationResult = clientApp.AcquireTokenSilent(azureAd.Value.Scope, userAccounts.FirstOrDefault()).ExecuteAsync().Result;
            }

            // Service Principal auth is the recommended by Microsoft to achieve App Owns Data Power BI embedding
            else if (azureAd.Value.AuthenticationMode.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificUrl = azureAd.Value.AuthorityUri.Replace("organizations", azureAd.Value.TenantId);

                // Create a confidential client to authorize the app with the AAD app
                IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                .Create(azureAd.Value.ClientId)
                                                                                .WithClientSecret(azureAd.Value.ClientSecret)
                                                                                .WithAuthority(tenantSpecificUrl)
                                                                                .Build();
                // Make a client call if Access token is not available in cache
                authenticationResult = clientApp.AcquireTokenForClient(azureAd.Value.Scope).ExecuteAsync().Result;
            }

            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Failed to authenticate with Power BI");
            }

            return authenticationResult.AccessToken;
        }
    }
}
