using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers.Agent
{
    public class AuditWithdrawalsController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(string despoitno, int status, string reason="")
        {
            var db = ContextDB.Context();
            var query = db.QuerySingle("exec PROC_AuditOrderDepositByAgent @0,@1,@2", despoitno, status, reason);
            if (query.status_code == 1)
            {
                model.message = "提现发起成功";
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
