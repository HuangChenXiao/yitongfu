using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;


namespace WebAPI.Controllers.Admin
{
    public class OrderUpdateByAdminController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(string orderno, string reason, string panyno = "")
        {
            var db = ContextDB.Context();
            var query = db.QuerySingle("exec PROC_OrderUpdateByAdmin @0,@1,@2", orderno, panyno, reason);
            if (query.status_code == 1)
            {
                model.message = query.message;
                model.status_code = 200;
            }
            else
            {
                model.message = query.message;
                model.status_code = 401;
            }

            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}