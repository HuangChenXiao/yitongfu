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
    public class BusinessCheckBalanceController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int businessid=0)
        {
            var db = ContextDB.Context();
            var query = db.Query("exec PROC_BusinessCheckBalance @0", businessid).ToList();
            if (query.Count > 0)
            {
                model.message = "查询成功";
                model.status_code = 200;
                model.data = query;
            }
            else
            {
                model.message = "暂无数据";
                model.status_code = 200;
            }

            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
