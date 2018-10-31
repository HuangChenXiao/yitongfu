using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace WebAPI.Controllers
{
    public class testController : ApiController
    {
        [ResponseType(typeof(Dictionary<string, object>))]
        public ResponseMessageResult Post(Dictionary<string, object> info) {
            var a = info;
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)200, ""));
        }
    }
}
