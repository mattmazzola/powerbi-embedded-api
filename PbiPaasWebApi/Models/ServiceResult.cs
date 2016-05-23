using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PbiPaasWebApi.Models
{
    public class ServiceResult
    {
        public string IdToken { get; set; }

        public AuthenticationResult AuthenticationResult { get; set; }
    }
}