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
    public class UnionBalanceController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int businessid)
        {
            var db = ContextDB.Context();
            var query = db.QuerySingle("exec proc_unionbusinessbalance @0", businessid);
            if (query.status_code == 1)
            {
                model.message = "操作成功";
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
