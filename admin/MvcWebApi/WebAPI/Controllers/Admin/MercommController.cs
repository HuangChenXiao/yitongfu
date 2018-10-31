using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers.Admin
{
    public class MercommController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int merchantid, string keyword, int page, int pagesize, string begindate = null, string enddate = null)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_mercommList @0,@1,@2,@3,@4,@5", begindate, enddate, merchantid, keyword, page, pagesize).ToList();

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
        public ResponseMessageResult Get(int merchantid, decimal comm_amount, int type, string memo)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.QuerySingle("exec PROC_Createmercomm @0,@1,@2,@3,@4", merchantid, comm_amount, type, memo, jwtmodel.username);

                if (query.status == 1)
                {
                    model.message = "操作成功";
                    model.status_code = 200;
                    model.data = query;
                }
                else
                {
                    model.message = "操作失败";
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

    }
}
