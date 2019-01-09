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
    public class LifeCircleController : ApiController
    {
        private ytfEntities db = new ytfEntities();
        JsonModel model = new JsonModel();
        // GET: api/Admin 用户登录
        public ResponseMessageResult Get(string code, string password)
        {
            var temp = from a in db.sh_business_appinfo select a;
            password = BaseHelper.Md5Hash(password);
            model.data = temp.Where(o => o.sh_mobile == code && o.sh_password == password && o.status==true).FirstOrDefault();
            if (model.data != null)
            {
                JwtModel jwtmodel = new JwtModel();
                jwtmodel.userid = model.data.id;
                jwtmodel.usercode = model.data.sh_mobile;
                jwtmodel.username = model.data.sh_businessname;
                jwtmodel.isadmin = true;
                jwtmodel.rolecode = "admin";
                JwtHelper.setToken(jwtmodel);
                return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)jwtmodel.status_code, jwtmodel));
            }
            else
            {
                model.message = "用户名或密码错误";
                model.status_code = 401;
                return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
            }
        }
        [ResponseType(typeof(sy_admin))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var sy_admin = from a in db.sh_business_appinfo
                               where a.id == jwtmodel.userid
                               select new
                               {
                                   a.id,
                                   a.sh_appid,
                                   a.sh_appsecret,
                                   a.sh_storeid,
                                   a.sh_storename,
                                   a.sh_businessname,
                                   a.sh_mobile,
                                   a.sh_balance,
                                   a.sh_commission,
                                   a.sh_commratio,
                                   a.agid,
                                   a.status
                               };
                if (sy_admin == null)
                {
                    model.message = "暂无数据";
                    model.status_code = 200;
                }
                else
                {
                    model.data = sy_admin.FirstOrDefault();
                    model.message = "查询成功";
                    model.status_code = 200;
                }
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
