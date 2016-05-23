using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PbiPaasWebApi.Models
{
    public class AuthenticationCode
    {
        public string Code { get; set; }
        public string RedirectUri { get; set; }
    }
}