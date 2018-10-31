using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers.LifeCircle
{
    public class LifeCircleQuickLogonController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        /// <summary>
        /// 代理商一键登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseMessageResult Getsy_agent(string username, string password, string sign)
        {
            string chk_sign = BaseHelper.Md5Hash(username + password + "buyunchina");
            JwtModel jwtmodel = new JwtModel();
            if (chk_sign == sign)
            {
                var temp = from a in db.sh_business_appinfo select a;
                //password = BaseHelper.Md5Hash(password);
                model.data = temp.Where(o => o.sh_mobile == username && o.sh_password == password && o.status == true).FirstOrDefault();
                if (model.data != null)
                {
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
            else
            {
                model.message = "用户名或密码错误";
                model.status_code = 401;
                return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
            }
        }
    }
}
