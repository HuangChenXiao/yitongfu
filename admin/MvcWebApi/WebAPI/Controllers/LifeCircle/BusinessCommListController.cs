using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class BusinessCommListController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(int status, int page, int pagesize, int sh_businessid=0, string begindate = "", string enddate = "")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec sh_businesscomm_list @0,@1,@2,@3,@4,@5", begindate, enddate,sh_businessid, status, page, pagesize).ToList();

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
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(string commorderno, int status, string reason)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.QuerySingle("exec sh_businesscomm_audit @0,@1,@2", commorderno, status, reason);

                if (query.status_code == 1)
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
