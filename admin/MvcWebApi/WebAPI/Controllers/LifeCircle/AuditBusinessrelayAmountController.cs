using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers.LifeCircle
{
    public class AuditBusinessrelayAmountController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult GetDespoitOrderList(int id, int status, string reason)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.QuerySingle("exec sh_auditbusinessrelayamount @0,@1,@2", id, status, reason);

                if (query.status_code ==1)
                {
                    model.message = query.message;
                    model.status_code = 200;
                    model.data = query;
                }
                else
                {
                    model.message = query.message;
                    model.status_code = 401;
                }
                db.Dispose();
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
