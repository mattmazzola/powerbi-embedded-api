using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using PbiPaasWebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PbiPaasWebApi.Controllers
{

    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenController : ApiController
    {
        static readonly string ClientId = ConfigurationManager.AppSettings["aad:ClientId"];
        static readonly string ClientSecret = ConfigurationManager.AppSettings["aad:ClientSecret"];
        static readonly string AzureServiceResourceId = ConfigurationManager.AppSettings["aad:AzureServiceResourceId"];
        static readonly string AzureServiceEndpointUri = ConfigurationManager.AppSettings["aad:AzureServiceEndpointUri"];
        static readonly string AzureGraphResourceId = ConfigurationManager.AppSettings["aad:AzureGraphResourceId"];
        static readonly string AzureGraphEndpointUri = ConfigurationManager.AppSettings["aad:AzureGraphEndpointUri"];
        static readonly string MicrosoftGraphResourceId = ConfigurationManager.AppSettings["aad:MicrosoftGraphResourceId"];
        static readonly string MicrosoftGraphEndpointUri = ConfigurationManager.AppSettings["aad:MicrosoftGraphEndpointUri"];
        static readonly string PowerBiResourceId = ConfigurationManager.AppSettings["aad:PowerBiResourceId"];
        static readonly string PowerBiEndpointUri = ConfigurationManager.AppSettings["aad:PowerBiEndpointUri"];
        static readonly Uri RedirectUri = new Uri(ConfigurationManager.AppSettings["aad:RedirectUri"]);
        static readonly AuthenticationContext authenticationContest = new AuthenticationContext("https://login.microsoftonline.com/common", new TokenCache());
        static readonly ClientCredential clientCredentials = new ClientCredential(ClientId, ClientSecret);
        static readonly Dictionary<string, string> services = new Dictionary<string, string>()
        {
            { AzureServiceResourceId, AzureServiceEndpointUri },
            { AzureGraphResourceId, AzureGraphEndpointUri },
            { MicrosoftGraphResourceId, MicrosoftGraphEndpointUri },
            { PowerBiResourceId, PowerBiEndpointUri },
        };

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody]Models.AuthenticationCode code)
        {
            var authenticationResult = await GetAuthenticationResponse((string resourceId) => authenticationContest.AcquireTokenByAuthorizationCodeAsync(code.Code, RedirectUri, clientCredentials, resourceId));

            return Ok(authenticationResult);
        }

        public async Task<IHttpActionResult> Get()
        {
            Models.AuthenticationResponse authenticationResult;

            try
            {
                authenticationResult = await GetAuthenticationResponse((string resourceId) => authenticationContest.AcquireTokenSilentAsync(resourceId, ClientId));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(authenticationResult);
        }

        private async Task<Models.AuthenticationResponse> GetAuthenticationResponse(Func<string, Task<AuthenticationResult>> getAuthenticationResult)
        {
            var authenticationResultTasks = services
                .Select(service =>
                {
                    return Task.Run(async () =>
                    {
                        var authenticationResult = await getAuthenticationResult(service.Key);

                        return new Models.ServiceResult()
                        {
                            IdToken = authenticationResult.IdToken,
                            AuthenticationResult = new Models.AuthenticationResult()
                            {
                                AccessToken = authenticationResult.AccessToken,
                                EndpointUri = service.Value,
                                ResourceId = service.Key
                            }
                        };
                    });
                });

            var authenticationResults = await Task.WhenAll(authenticationResultTasks);

            var authenticationResponse = new Models.AuthenticationResponse();
            authenticationResponse.IdToken = authenticationResults.First().IdToken;
            authenticationResponse.Resources = authenticationResults.Select(x => x.AuthenticationResult).ToList();

            return authenticationResponse;
        }
    }
}