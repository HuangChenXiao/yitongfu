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

namespace WebAPI.Controllers.Merchant
{
    public class FaMerchantController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();

        /// <summary>
        /// 虚拟商户登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseMessageResult Getsy_merchant(string code, string password)
        {
            var temp = from a in db.sy_merchant
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
        /// 获取虚拟商户信息
        /// </summary>
        /// <returns></returns>
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_merchant()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_merchant
                           where a.id == jwtmodel.userid
                           select a;
                model.total = temp.Count();
                model.data = temp.FirstOrDefault();

                if (model.data!=null)
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

        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        [ResponseType(typeof(fa_business_basic))]
        public ResponseMessageResult Getfa_business_basic(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           join b in db.sy_merchant on a.merchantid equals b.id
                           join c in db.sy_agent on b.agid equals c.id
                           where (a.merchantName.Contains(code) || string.IsNullOrEmpty(code))
                           && b.id == jwtmodel.userid
                           select new
                           {
                               a.id,
                               a.code,
                               a.password,
                               a.merchantid,
                               a.merchantName,
                               a.shortName,
                               a.handleType,
                               a.city,
                               a.merchantAddress,
                               a.servicePhone,
                               a.orgCode,
                               a.merchantType,
                               a.category,
                               a.corpmanName,
                               a.corpmanId,
                               a.corpmanPhone,
                               a.corpmanMobile,
                               a.corpmanEmail,
                               a.bankCode,
                               a.bankaccountNo,
                               a.bankName,
                               a.bankaccountName,
                               a.autoCus,
                               a.remark,
                               a.licenseNo,
                               a.taxRegisterNo,
                               a.appId,
                               a.appSecret,
                               a.status,
                               a.balance,
                               a.addtime,
                               a.adduser,
                               a.updatetime,
                               a.updateuser,
                               merchantname = b.name,
                               agname = c.name
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
        // PUT: api/Admin/5
        [ResponseType(typeof(fa_business_basic))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Putfa_business_basic(fa_business_basic fa_business_basic)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var count = db.fa_business_basic.Where(o => o.merchantName == fa_business_basic.merchantName && fa_business_basic.merchantid == o.merchantid && o.id != fa_business_basic.id).Count();
                if (count > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
                    var info = db.fa_business_basic.Find(fa_business_basic.id);
                    if (!string.IsNullOrEmpty(fa_business_basic.password))
                    {
                        info.password = BaseHelper.Md5Hash(fa_business_basic.password);
                    }
                    info.merchantid = fa_business_basic.merchantid;
                    info.merchantName = fa_business_basic.merchantName;
                    info.shortName = fa_business_basic.shortName;
                    info.handleType = fa_business_basic.handleType;
                    info.city = fa_business_basic.city;
                    info.merchantAddress = fa_business_basic.merchantAddress;
                    info.servicePhone = fa_business_basic.servicePhone;
                    info.orgCode = fa_business_basic.orgCode;
                    info.merchantType = fa_business_basic.merchantType;
                    info.category = fa_business_basic.category;
                    info.corpmanName = fa_business_basic.corpmanName;
                    info.corpmanId = fa_business_basic.corpmanId;
                    info.corpmanPhone = fa_business_basic.corpmanPhone;
                    info.corpmanMobile = fa_business_basic.corpmanMobile;
                    info.corpmanEmail = fa_business_basic.corpmanEmail;
                    info.bankCode = fa_business_basic.bankCode;
                    info.bankName = fa_business_basic.bankName;
                    info.bankaccountNo = fa_business_basic.bankaccountNo;
                    info.bankaccountName = fa_business_basic.bankaccountName;
                    info.autoCus = fa_business_basic.autoCus;
                    info.remark = fa_business_basic.remark;
                    info.licenseNo = fa_business_basic.licenseNo;
                    info.taxRegisterNo = fa_business_basic.taxRegisterNo;
                    info.appId = fa_business_basic.appId;
                    info.appSecret = fa_business_basic.appSecret;
                    info.status = fa_business_basic.status;
                    info.updatetime = DateTime.Now;
                    info.updateuser = jwtmodel.username;
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

        // POST: api/fa_business_basic
        [ResponseType(typeof(fa_business_basic))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Postfa_business_basic(fa_business_basic fa_business_basic)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.fa_business_basic.Where(o => o.merchantName == fa_business_basic.merchantName && fa_business_basic.merchantid==o.merchantid).Count();
                if (info > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
                    fa_business_basic.addtime = DateTime.Now;
                    fa_business_basic.adduser = jwtmodel.username;
                    fa_business_basic.password = BaseHelper.Md5Hash(fa_business_basic.password);
                    db.fa_business_basic.Add(fa_business_basic);
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