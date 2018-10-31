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

namespace WebAPI.Controllers.Agent
{
    public class ByAgentController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult GetAgentwithdrawlistbyagent(int status, int page, int pagesize, string begindate = "", string enddate = "")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_Agentwithdrawlistbyagent @0,@1,@2,@3,@4,@5", begindate, enddate,jwtmodel.userid, status, page, pagesize).ToList();

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
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        public ResponseMessageResult GetOrderList(string keyword, int page, int pagesize, int status, string begindate = "", string enddate = "", string businesspasstype = "", int paytype = 0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_OrderList @0,@1,@2,@3,@4,@5,@6,@7,@8,@9", begindate, enddate, jwtmodel.userid, 0, keyword, status, page, pagesize, businesspasstype, paytype).ToList();

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
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        [ResponseType(typeof(po_agentwithdraw))]
        public ResponseMessageResult PutAgentwithdrawlistbyagent(po_agentwithdraw info)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_Agentaddwithdraw @0,@1",  jwtmodel.userid, info.commission).First();

                if (query.state == 1)
                {
                    model.message = "操作成功";
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
