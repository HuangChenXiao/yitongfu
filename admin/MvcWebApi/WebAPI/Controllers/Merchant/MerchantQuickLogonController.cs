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

namespace WebAPI.Controllers.Agent
{
    public class MerchantQuickLogonController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        /// <summary>
        /// 虚拟商户一键登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseMessageResult Getsy_merchant(string username, string password, string sign)
        {
            string chk_sign = BaseHelper.Md5Hash(username + password + "buyunchina");
            JwtModel jwtmodel = new JwtModel();
            if (chk_sign == sign)
            {
                var temp = from a in db.sy_merchant
                           select a;
                model.data = temp.Where(o => o.name == username && o.password == password && o.user_status == 1).FirstOrDefault();
                if (model.data != null)
                {
                    jwtmodel.userid = model.data.id;
                    jwtmodel.usercode = model.data.name;
                    jwtmodel.username = model.data.name;
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
