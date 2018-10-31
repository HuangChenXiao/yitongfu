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

namespace WebAPI.Controllers.Admin
{
    [WebApiActionDebugFilter]
    public class PayPassListController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int page, int pagesize, string status, string keyword = "")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_PayPassList @0,@1,@2,@3", keyword, status, page, pagesize).ToList();

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
        public ResponseMessageResult Post(string alipayappid, string rsaprivate, string rsapublic, decimal dayamount, string memo, bool enable, int id = 0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                if (id <= 0)
                {
                    var query = db.QuerySingle("exec PROC_PayPassAdd @0,@1,@2,@3,@4", alipayappid, rsaprivate, rsapublic, dayamount, memo);

                    if (query.status_code ==1)
                    {
                        model.message = "新增成功";
                        model.status_code = 200;
                        model.data = query;
                    }
                    else
                    {
                        model.message = "暂无数据";
                        model.status_code = 200;
                    }
                }
                else
                {
                    var query = db.QuerySingle("exec PROC_PayPassUpdate @0,@1,@2,@3,@4,@5,@6", id, alipayappid, rsaprivate, rsapublic, dayamount, memo, enable);

                    if (query.status_code == 1)
                    {
                        model.message = "修改成功";
                        model.status_code = 200;
                        model.data = query;
                    }
                    else
                    {
                        model.message = "暂无数据";
                        model.status_code = 200;
                    }
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
