using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ModelDb;
using System.Collections;
using System.Web;
using WebAPI.Models;
using System.Web.Http.Results;
using Com;
using WebAPI;

namespace WebAPI.Controllers.LifeCircle
{
    public class LifeCircleOrderController : ApiController
    {
        private ytfEntities db = new ytfEntities();
        JsonModel model = new JsonModel();
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(string keyword, int status, int page, int pagesize, string begindate = "", string enddate = "", string sh_businessid = "", int paytype = 0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec sh_orderlist @0,@1,@2,@3,@4,@5,@6,@7", begindate, enddate, jwtmodel.userid, keyword, status, page, pagesize, paytype).ToList();

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
    }
}
