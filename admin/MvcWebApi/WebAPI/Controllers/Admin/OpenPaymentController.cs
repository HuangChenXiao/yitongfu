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
    public class OpenPaymentController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        public ResponseMessageResult Getfa_open_payment(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_open_payment
                           join b in db.fa_business_basic on a.merchantId equals b.appId
                           join c in db.sy_merchant on b.merchantid equals c.id
                           join d in db.sy_agent on c.agid equals d.id
                           where (a.merchantId.Contains(code) || string.IsNullOrEmpty(code))
                           select new
                           {
                               a.id,
                               a.merchantId,
                               a.handleType,
                               a.cycleValue,
                               a.busiCode,
                               a.futureRateType,
                               a.futureRateValue,
                               a.futureMinAmount,
                               a.futureMaxAmount,
                               a.agencyId,
                               a.wechat_status,
                               a.wechat_rate,
                               a.alipay_status,
                               a.alipay_rate,
                               a.daipay_status,
                               a.daipay_rate,
                               a.qqpay_status,
                               a.qqpay_rate,
                               a.jdpay_status,
                               a.jdpay_rate,
                               a.unionpay_status,
                               a.unionpay_rate,
                               a.addtime,
                               a.adduser,
                               a.updatetime,
                               a.updateuser,
                               a.isaudit,
                               b.shortName,
                               merchantname = c.name,
                               agname = d.name
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
        [ResponseType(typeof(fa_open_payment))]
        public ResponseMessageResult Putfa_open_payment(fa_open_payment fa_open_payment)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                db.Entry(fa_open_payment).State = EntityState.Modified;
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

        // POST: api/fa_open_payment
        [ResponseType(typeof(fa_open_payment))]
        public ResponseMessageResult Postfa_open_payment(fa_open_payment fa_open_payment)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                fa_open_payment.addtime = DateTime.Now;
                fa_open_payment.adduser = jwtmodel.username;
                db.fa_open_payment.Add(fa_open_payment);
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
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        // DELETE: api/Router/5
        [ResponseType(typeof(fa_open_payment))]
        public ResponseMessageResult Deletefa_open_payment(int id)
        {

            fa_open_payment fa_open_payment = db.fa_open_payment.Find(id);
            if (fa_open_payment == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.fa_open_payment.Remove(fa_open_payment);
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