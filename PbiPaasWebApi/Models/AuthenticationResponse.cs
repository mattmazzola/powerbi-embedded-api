using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PbiPaasWebApi.Models
{
    public class AuthenticationResponse
    {
        public List<AuthenticationResult> Resources { get; set; }

        public string IdToken { get; set; }
    }
}