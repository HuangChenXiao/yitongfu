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
    public class AgentController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_agent(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_agent
                           where (a.name.Contains(code) || string.IsNullOrEmpty(code))
                           select a;
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
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_agent()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            var sy_agent = from a in db.sy_agent
                           where a.id == jwtmodel.userid
                           select a;
            if (sy_agent == null)
            {
                model.message = "暂无数据";
                model.status_code = 200;
            }
            else
            {
                model.data = sy_agent.FirstOrDefault();
                model.message = "查询成功";
                model.status_code = 200;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        [WebApiActionDebugFilter]
        public ResponseMessageResult GetOrderList(string keyword, int status, int page, int pagesize, string begindate = "", string enddate = "", string businesspasstype = "", int paytype=0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_OrderList @0,@1,@2,@3,@4,@5,@6,@7,@8,@9", begindate, enddate, 0, 0, keyword, status, page, pagesize, businesspasstype, paytype).ToList();

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
        // PUT: api/Admin/5
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Putsy_agent(sy_agent sy_agent)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var count = db.sy_agent.Where(o => o.name == sy_agent.name && o.id != sy_agent.id).Count();
                if (count > 0)
                {
                    model.message = "用户编码已经存在";
                    model.status_code = 401;
                }
                else
                {
                    var info = db.sy_agent.Find(sy_agent.id);
                    if (!string.IsNullOrEmpty(sy_agent.password))
                    {
                        info.password = BaseHelper.Md5Hash(sy_agent.password);
                    }
                    info.name = sy_agent.name;
                    info.ratio = sy_agent.ratio;
                    info.account_name = sy_agent.account_name;
                    info.card_number = sy_agent.card_number;
                    info.bank_accounts = sy_agent.bank_accounts;
                    info.opening_address = sy_agent.opening_address;
                    info.opening_point = sy_agent.opening_point;
                    info.agency_level = sy_agent.agency_level;
                    info.direct_identity = sy_agent.direct_identity;
                    info.agent_rights = sy_agent.agent_rights;
                    info.agency_amount = sy_agent.agency_amount;
                    info.agency_expiration_date = sy_agent.agency_expiration_date;
                    info.user_status = sy_agent.user_status;
                    info.remarks = sy_agent.remarks;
                    info.corporate_name = sy_agent.corporate_name;
                    info.province = sy_agent.province;
                    info.city = sy_agent.city;
                    info.area = sy_agent.area;
                    info.industry_owned = sy_agent.industry_owned;
                    info.main_business = sy_agent.main_business;
                    info.company_number = sy_agent.company_number;
                    info.annual_turnover = sy_agent.annual_turnover;
                    info.contacts = sy_agent.contacts;
                    info.contact_number = sy_agent.contact_number;
                    info.mobile_phone = sy_agent.mobile_phone;
                    info.qq_number = sy_agent.qq_number;
                    info.mail_box = sy_agent.mail_box;
                    info.proof_document = sy_agent.proof_document;
                    info.company_logo = sy_agent.company_logo;
                    info.is_alipay = sy_agent.is_alipay;
                    info.is_jd = sy_agent.is_jd;
                    info.is_t0 = sy_agent.is_t0;
                    info.updatetime = DateTime.Now;
                    info.updateuser = jwtmodel.username;
                    info.domain = sy_agent.domain;
                    info.bus_domain = sy_agent.bus_domain;
                    //db.Entry(sy_agent).State = EntityState.Modified;
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

        // POST: api/sy_agent
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Postsy_agent(sy_agent sy_agent)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_agent.Where(o => o.name == sy_agent.name).Count();
                if (info > 0)
                {
                    model.message = "商户名称已经存在";
                    model.status_code = 401;
                }
                else
                {
                    sy_agent.addtime = DateTime.Now;
                    sy_agent.adduser= jwtmodel.username;
                    sy_agent.password = BaseHelper.Md5Hash(sy_agent.password);
                    db.sy_agent.Add(sy_agent);
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
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Deletesy_agent(int id)
        {

            sy_agent sy_agent = db.sy_agent.Find(id);
            if (sy_agent == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.sy_agent.Remove(sy_agent);
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