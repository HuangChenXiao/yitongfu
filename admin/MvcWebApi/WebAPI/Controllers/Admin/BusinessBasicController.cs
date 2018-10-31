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
namespace WebAPI.Controllers.Admin
{
    [WebApiActionDebugFilter]
    public class BusinessBasicController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        [ResponseType(typeof(fa_business_basic))]
        public ResponseMessageResult Getfa_business_basic(int page, int pagesize, string code, int merchantid)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           join b in db.sy_merchant on a.merchantid equals b.id
                           join c in db.sy_agent on a.agentid equals c.id
                           where (a.merchantName.Contains(code) || b.name.Contains(code) || c.name.Contains(code) || a.shortName.Contains(code) || string.IsNullOrEmpty(code))
                           && (a.merchantid == merchantid || merchantid <= 0)
                           select new
                           {
                               a.balance,
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
                               a.bankName,
                               a.bankaccountNo,
                               a.bankaccountName,
                               a.autoCus,
                               a.remark,
                               a.licenseNo,
                               a.taxRegisterNo,
                               a.appId,
                               a.appSecret,
                               a.status,
                               a.addtime,
                               a.adduser,
                               a.updatetime,
                               a.updateuser,
                               a.isaudit,
                               mername = b.name,
                               agname = c.name,
                               a.addrType,
                               a.contactType,
                               a.mcc,
                               a.licenseType,
                               a.contactMan,
                               a.telNo,
                               a.mobilePhone,
                               a.email,
                               a.licenseBeginDate,
                               a.licenseEndDate,
                               a.licenseRange,
                               a.ratio,
                               a.minAmount,
                               a.maxAmount,
                               a.disabletime,
                               a.enabletime,
                               a.agentid,
                               a.depositratio,
                               c.bus_domain,
                               a.businesspasstype,
                               a.wechatratio,
                               a.alipayratio,
                               a.unionratio,
                               a.alipayaccount
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
        public ResponseMessageResult Getfa_business_basic()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           select a;
                model.total = temp.Count();
                model.data = temp.OrderByDescending(o => o.id).ToList();

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
        public ResponseMessageResult Putfa_business_basic(fa_business_basic fa_business_basic)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.fa_business_basic.Find(fa_business_basic.id);
                if (!string.IsNullOrEmpty(fa_business_basic.password))
                {
                    info.password = BaseHelper.Md5Hash(fa_business_basic.password);
                }
                info.isaudit = fa_business_basic.isaudit;
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
                info.addrType = fa_business_basic.addrType;
                info.contactType = fa_business_basic.contactType;
                info.mcc = fa_business_basic.mcc;
                info.licenseType = fa_business_basic.licenseType;
                info.contactMan = fa_business_basic.contactMan;
                info.telNo = fa_business_basic.telNo;
                info.mobilePhone = fa_business_basic.mobilePhone;
                info.email = fa_business_basic.email;
                info.licenseBeginDate = fa_business_basic.licenseBeginDate;
                info.licenseEndDate = fa_business_basic.licenseEndDate;
                info.licenseRange = fa_business_basic.licenseRange;
                info.ratio = fa_business_basic.ratio;
                info.minAmount = fa_business_basic.minAmount;
                info.maxAmount = fa_business_basic.maxAmount;
                info.disabletime = fa_business_basic.disabletime;
                info.enabletime = fa_business_basic.enabletime;
                info.updatetime = DateTime.Now;
                info.updateuser = jwtmodel.username;
                info.agentid = fa_business_basic.agentid;
                info.depositratio = fa_business_basic.depositratio;
                info.businesspasstype = fa_business_basic.businesspasstype;
                info.wechatratio = fa_business_basic.wechatratio;
                info.alipayratio = fa_business_basic.alipayratio;
                info.unionratio = fa_business_basic.unionratio;
                info.alipayaccount = fa_business_basic.alipayaccount;
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
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        // POST: api/fa_business_basic
        [ResponseType(typeof(fa_business_basic))]
        public ResponseMessageResult Postfa_business_basic(fa_business_basic fa_business_basic)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                //var bus = db.fa_business_basic.Where(o => o.agentid == fa_business_basic.agentid && o.id != fa_business_basic.id && o.status == 1).OrderByDescending(o => o.maxAmount).FirstOrDefault();
                //var maxAmount = bus != null ? bus.maxAmount : 0;
                //if (fa_business_basic.minAmount <= maxAmount)
                //{
                //    model.message = "收款最小金额不能小于已经存在的收款最大金额";
                //    model.status_code = 401;
                //    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
                //}
                var info = db.fa_business_basic.Where(o => o.merchantName == fa_business_basic.merchantName && o.agentid == fa_business_basic.agentid).Count();
                if (info > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {

                    var strDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    fa_business_basic.appId = strDateTime;
                    fa_business_basic.appSecret = BaseHelper.Md5Hash(strDateTime + "buyunchina");
                    fa_business_basic.password = BaseHelper.Md5Hash(fa_business_basic.password);
                    fa_business_basic.addtime = DateTime.Now;
                    fa_business_basic.adduser = jwtmodel.username;
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

        // DELETE: api/Router/5
        [ResponseType(typeof(fa_business_basic))]
        public ResponseMessageResult Deletefa_business_basic(int id)
        {

            fa_business_basic fa_business_basic = db.fa_business_basic.Find(id);
            if (fa_business_basic == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.fa_business_basic.Remove(fa_business_basic);
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