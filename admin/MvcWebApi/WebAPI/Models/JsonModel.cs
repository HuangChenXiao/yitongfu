using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class JsonModel
    {
        public string message { get; set; }
        public int status_code { get; set; }
        public dynamic data { get; set; }
        public int total { get; set; }
    }
}