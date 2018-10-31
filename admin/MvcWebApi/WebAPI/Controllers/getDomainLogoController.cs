using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class getDomainLogoController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(string domain, int type)
        {
            var db = ContextDB.Context();
            if (type == 1)//代理商
            {
                var ag_list = db.QuerySingle("select company_logo,domain from [dbo].[sy_agent] where domain=@0", domain);
                model.message = "查询成功";
                model.status_code = 200;
                model.data = ag_list;
            }
            else if (type == 2)//商户
            {
                var ag_list = db.QuerySingle("select company_logo,bus_domain from [dbo].[sy_agent] where bus_domain=@0", domain);
                model.message = "查询成功";
                model.status_code = 200;
                model.data = ag_list;
            }
            else
            {
                model.message = "暂未设置跳转域名";
                model.status_code = 200;
            }
            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
