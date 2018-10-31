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

namespace WebAPI.Controllers.Merchant
{
    public class ByMerchantController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();

        /// <summary>
        /// 虚拟商户登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseMessageResult Get(string code, string password)
        {
            var temp = from a in db.fa_business_basic
                       select a;
            password = BaseHelper.Md5Hash(password);
            model.data = temp.Where(o => o.merchantName == code && o.password == password && o.status == 1).FirstOrDefault();
            if (model.data != null)
            {
                JwtModel jwtmodel = new JwtModel();
                jwtmodel.userid = model.data.id;
                jwtmodel.usercode = model.data.merchantName;
                jwtmodel.username = model.data.merchantName;
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
        /// <summary>
        /// 获取虚拟商户信息
        /// </summary>
        /// <returns></returns>
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           where a.id == jwtmodel.userid
                           select a;
                model.total = temp.Count();
                model.data = temp.FirstOrDefault();

                if (model.data != null)
                {
                    model.message = "查询成功";
                    model.status_code = 200;
                }
                else
                {
                    model.message = "暂无数据";
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
        public ResponseMessageResult GetOrderList(string keyword, int status, int page, int pagesize, string begindate = "", string enddate = "", int paytype = 0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_OrderList @0,@1,@2,@3,@4,@5,@6,@7,@8,@9", begindate, enddate, 0, jwtmodel.userid, keyword, status, page, pagesize,"", paytype).ToList();

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
        public ResponseMessageResult GetDespoitOrderList(int status, int page, int pagesize, string begindate = "", string enddate = "", string businesspasstype="")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_DespoitOrderList @0,@1,@2,@3,@4,@5,@6,@7", begindate, enddate, 0, jwtmodel.userid, status, page, pagesize, businesspasstype).ToList();

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
