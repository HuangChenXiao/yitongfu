using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    [WebApiActionDebugFilter]
    public class GetNotifyadminInfoController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(string orderno)
        {
            var db = ContextDB.Context();
            var query = db.QuerySingle("exec PROC_GetNotifyadminInfo @0", orderno);
            model.message = "成功";
            model.status_code = 200;
            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}