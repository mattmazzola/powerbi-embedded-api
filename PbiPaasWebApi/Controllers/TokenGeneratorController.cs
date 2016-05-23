using Microsoft.PowerBI.Security;
using PbiPaasWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PbiPaasWebApi.Controllers
{
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenGeneratorController : ApiController
    {
        [HttpPost]
        [Route("generateProvisionToken")]
        public IHttpActionResult GenerateProvisionToken([FromBody]CreateProvisionToken createProvisionToken)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            var provisionToken = PowerBIToken.CreateProvisionToken(createProvisionToken.WorkspaceCollectionName);
            return Ok(provisionToken.Generate(createProvisionToken.AccessKey));
        }

        [HttpPost]
        [Route("generateDevToken")]
        public IHttpActionResult GenerateDevToken([FromBody]CreateDevToken createDevToken)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            var devToken = PowerBIToken.CreateDevToken(createDevToken.WorkspaceCollectionName, createDevToken.WorkspaceId);
            return Ok(devToken.Generate(createDevToken.AccessKey));
        }

        [HttpPost]
        [Route("generateEmbedToken")]
        public IHttpActionResult GenerateEmbedToken([FromBody]CreateEmbedToken createEmbedToken)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            var reportEmbedToken = PowerBIToken.CreateReportEmbedToken(createEmbedToken.WorkspaceCollectionName, createEmbedToken.WorkspaceId, createEmbedToken.ReportId);
            return Ok(reportEmbedToken.Generate(createEmbedToken.AccessKey));
        }
    }
}
