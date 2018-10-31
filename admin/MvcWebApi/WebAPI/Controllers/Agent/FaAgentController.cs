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

namespace WebAPI.Controllers.Agent
{
    public class FaAgentController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        /// <summary>
        /// 代理商登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseMessageResult Getsy_agent(string code, string password)
        {
            var temp = from a in db.sy_agent
                       select a;
            password = BaseHelper.Md5Hash(password);
            model.data = temp.Where(o => o.name == code && o.password == password && o.user_status == 1).FirstOrDefault();
            if (model.data != null)
            {
                JwtModel jwtmodel = new JwtModel();
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
        /// <summary>
        /// 获取代理商信息
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_agent()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            var sy_merchant = from a in db.sy_agent
                              where a.id == jwtmodel.userid
                              select a;
            if (sy_merchant == null)
            {
                model.message = "暂无数据";
                model.status_code = 200;
            }
            else
            {
                model.data = sy_merchant.FirstOrDefault();
                model.message = "查询成功";
                model.status_code = 200;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_merchant(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_merchant
                           join b in db.sy_agent on a.agid equals b.id
                           where (a.name.Contains(code) || string.IsNullOrEmpty(code))
                           && a.agid == jwtmodel.userid
                           select new
                           {
                               a.id,
                               a.name,
                               a.agid,
                               a.user_status,
                               a.user_rights,
                               a.merchant_name,
                               a.province,
                               a.city,
                               a.area,
                               a.merchant_address,
                               a.contacts,
                               a.contact_information,
                               a.mail_box,
                               a.qq_number,
                               a.expiration_date,
                               a.enabletime,
                               a.disabletime,
                               a.payment_limit,
                               a.recharge_limit,
                               a.last_login_time,
                               a.addtime,
                               a.adduser,
                               a.appid,
                               a.appsecret,
                               agname = b.name,
                               a.comm_ratio,
                               a.company_logo,
                               a.domain
                               
                           };
                model.total = temp.Count();
                model.data = temp.OrderByDescending(s => s.id).Skip((page - 1) * pagesize).Take(pagesize).ToList();

                if (model.data.Count > 0)
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
        [ResponseType(typeof(sy_merchant))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Putsy_merchant(sy_merchant sy_merchant)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var count = db.sy_merchant.Where(o => o.name == sy_merchant.name && sy_merchant.agid == o.agid && o.id != sy_merchant.id).Count();
                if (count > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
                    var info = db.sy_merchant.Find(sy_merchant.id);
                    if (!string.IsNullOrEmpty(sy_merchant.password))
                    {
                        info.password = BaseHelper.Md5Hash(sy_merchant.password);
                    }
                    info.id = sy_merchant.id;
                    info.name = sy_merchant.name;
                    info.agid = jwtmodel.userid;
                    info.user_status = sy_merchant.user_status;
                    info.user_rights = sy_merchant.user_rights;
                    info.merchant_name = sy_merchant.merchant_name;
                    info.province = sy_merchant.province;
                    info.city = sy_merchant.city;
                    info.area = sy_merchant.area;
                    info.merchant_address = sy_merchant.merchant_address;
                    info.contacts = sy_merchant.contacts;
                    info.contact_information = sy_merchant.contact_information;
                    info.mail_box = sy_merchant.mail_box;
                    info.qq_number = sy_merchant.qq_number;
                    info.expiration_date = sy_merchant.expiration_date;
                    info.payment_limit = sy_merchant.payment_limit;
                    info.recharge_limit = sy_merchant.recharge_limit;
                    info.enabletime = sy_merchant.enabletime;
                    info.disabletime = sy_merchant.disabletime;
                    info.updatetime = DateTime.Now;
                    info.updateuser = jwtmodel.username;
                    info.comm_ratio = sy_merchant.comm_ratio;
                    info.domain = sy_merchant.domain;
                    info.company_logo = sy_merchant.company_logo;
                    try
                    {
                        model.message = "修改成功";
                        model.status_code = 200;
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        model.message = ex.Message;
                        model.status_code = 401;
                    }
                }
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        // POST: api/sy_merchant
        [ResponseType(typeof(sy_merchant))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Postsy_merchant(sy_merchant sy_merchant)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_merchant.Where(o => o.name == sy_merchant.name && sy_merchant.agid == o.agid).Count();
                if (info > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
                    var strDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    sy_merchant.appid = strDateTime;
                    sy_merchant.appsecret = BaseHelper.Md5Hash(strDateTime + "buyunchina");
                    sy_merchant.agid = jwtmodel.userid;
                    sy_merchant.addtime = DateTime.Now;
                    sy_merchant.adduser = jwtmodel.username;
                    sy_merchant.password = BaseHelper.Md5Hash(sy_merchant.password);
                    db.sy_merchant.Add(sy_merchant);
                    try
                    {
                        db.SaveChanges();
                        model.message = "新增成功";
                        model.status_code = 200;
                    }
                    catch (Exception ex)
                    {
                        model.message = ex.Message;
                        model.status_code = 401;
                    }
                }
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        // DELETE: api/Router/5
        [ResponseType(typeof(sy_merchant))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Deletesy_merchant(int id)
        {

            sy_merchant sy_merchant = db.sy_merchant.Find(id);
            if (sy_merchant == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.sy_merchant.Remove(sy_merchant);
            db.SaveChanges();
            model.message = "删除成功";
            model.status_code = 200;
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
