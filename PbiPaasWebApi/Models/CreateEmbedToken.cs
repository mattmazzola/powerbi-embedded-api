﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PbiPaasWebApi.Models
{
    public class CreateEmbedToken
    {
        [Required]
        public string WorkspaceCollectionName { get; set; }
        [Required]
        public string WorkspaceId { get; set; }
        [Required]
        public string ReportId { get; set; }
        [Required]
        public string AccessKey { get; set; }
    }
}