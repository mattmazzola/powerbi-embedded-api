using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PbiPaasWebApi.Models
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string EndpointUri { get; set; }
        public string ResourceId { get; set; }
        public string IdToken { get; set; }
    }
}