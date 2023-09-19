using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class OktaService : BaseService, IOktaService
    {
        public OktaService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<Tuple<bool, string>> CreateCTEPUser(string firstName, string lastName)
        {
            try
            {
                // Setup the call for creating a CTEP user
                var options = new RestClientOptions("https://theradexbeta.oktapreview.com")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("//api/v1/groups?q=Web%20Reporting-NCI", Method.Get);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "SSWS 00zGBSYjaCFyg1B8mcL7mPi9kWSb5ZCvCxaNEgBaL5");
                RestResponse response = await client.ExecuteAsync(request);

                return (new Tuple<bool, string>(true, "Success"));
            }
            catch (Exception ex)
            {
                return (new Tuple<bool, string>(false, ex.Message));
            }
        }

        public async Task<Tuple<bool, string>> CreateNonTEPUser(string firstName, string lastName)
        {
            try
            {
                // Setup the call for creating a CTEP user
                return (new Tuple<bool, string>(true, "Success"));
            }
            catch (Exception ex)
            {
                return (new Tuple<bool, string>(false, ex.Message));
            }
        }
    }
}
