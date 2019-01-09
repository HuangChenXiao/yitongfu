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
    public class FaBusinessBasicController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        public ResponseMessageResult Getfa_business_basic()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           join c in db.sy_agent on a.agentid equals c.id
                           where c.id == jwtmodel.userid
                           select a;
                model.total = temp.Count();
                model.data = temp.ToList();

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
                var info = db.fa_business_basic.Where(o => o.merchantName == fa_business_basic.merchantName).Count();
                if (info > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
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