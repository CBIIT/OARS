namespace TheradexPortal.Data.PowerBI
{
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Client;
    using System;
    using System.Linq;
    using System.Security;
    using System.Text.Json;
    using TheradexPortal.Data.PowerBI.Models;

    public class AadService
    {
        private readonly AzureAd? azureAd;

        public AadService(IOptions<AzureAd> azureAd)
        {
            var azureCredsString = Environment.GetEnvironmentVariable("POWERBI_CREDENTIALS");
            if (!string.IsNullOrWhiteSpace(azureCredsString))
                this.azureAd = JsonSerializer.Deserialize<AzureAd>(azureCredsString);
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

            AuthenticationResult authenticationResult = null;
            if (azureAd.AuthenticationMode.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                // Create a public client to authorize the app with the AAD app
                IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(azureAd.ClientId).WithAuthority(azureAd.AuthorityUri).Build();
                var userAccounts = clientApp.GetAccountsAsync().Result;
                // Retrieve Access token from cache if available
                authenticationResult = clientApp.AcquireTokenSilent(azureAd.Scope, userAccounts.FirstOrDefault()).ExecuteAsync().Result;
            }

            // Service Principal auth is the recommended by Microsoft to achieve App Owns Data Power BI embedding
            else if (azureAd.AuthenticationMode.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificUrl = azureAd.AuthorityUri.Replace("organizations", azureAd.TenantId);

                // Create a confidential client to authorize the app with the AAD app
                IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                .Create(azureAd.ClientId)
                                                                                .WithClientSecret(azureAd.ClientSecret)
                                                                                .WithAuthority(tenantSpecificUrl)
                                                                                .Build();
                // Make a client call if Access token is not available in cache
                authenticationResult = clientApp.AcquireTokenForClient(azureAd.Scope).ExecuteAsync().Result;
            }

            return authenticationResult.AccessToken;
        }
    }
}
