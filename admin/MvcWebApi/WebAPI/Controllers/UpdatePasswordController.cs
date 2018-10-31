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
    public class UpdatePasswordController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int id, string password, int type)
        {
            var db = ContextDB.Context();
            if (type == 1)//代理商
            {
                var ag_list = db.QuerySingle("update sy_agent set password=@0 where id=@1", BaseHelper.Md5Hash(password), id);
                model.message = "修改成功";
                model.status_code = 200;
            }
            else if (type == 2)//商户
            {
                var ag_list = db.QuerySingle("update fa_business_basic set password=@0 where id=@1", BaseHelper.Md5Hash(password), id);
                model.message = "修改成功";
                model.status_code = 200;
            }
            else
            {
                model.message = "修改失败";
                model.status_code = 200;
            }
            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
