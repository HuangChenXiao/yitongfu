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
    public class MeMemberController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        public ResponseMessageResult Getme_member(int page, int pagesize, string code, int agentid = 0, int businessid = 0, bool? enable=null)
        {
            var temp = from a in db.me_member_view
                       where (a.shortName.Contains(code) || a.phone.Contains(code) || a.alipayname.Contains(code) || a.wechatname.Contains(code) || string.IsNullOrEmpty(code))
                       && (agentid == 0 || a.agent_id == agentid)
                       && (businessid == 0 || a.bus_id == businessid)
                       && (enable == null || a.enable == enable)
                       select new
                       {
                           a.memberid,
                           a.phone,
                           a.alipayname,
                           a.wechatname,
                           a.iswechat,
                           a.isalipay,
                           a.alipayamount,
                           a.alipayuseamount,
                           a.wechatamount,
                           a.wechatuseamount,
                           a.enable,
                           a.businessid,
                           a.userid,
                           a.shortName,
                           a.agent_name
                       };
            model.total = temp.Count();
            model.data = temp.OrderByDescending(s => s.memberid).Skip((page - 1) * pagesize).Take(pagesize).ToList();

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
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        // PUT: api/Admin/5
        [ResponseType(typeof(me_member))]
        public ResponseMessageResult Putme_member(me_member me_member)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            db.Entry(me_member).State = EntityState.Modified;
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
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        // POST: api/me_member
        [ResponseType(typeof(me_member))]
        public ResponseMessageResult Postme_member(me_member me_member)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            db.me_member.Add(me_member);
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
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        // DELETE: api/Router/5
        [ResponseType(typeof(me_member))]
        public ResponseMessageResult Deleteme_member(int autoid)
        {

            me_member me_member = db.me_member.Find(autoid);
            if (me_member == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.me_member.Remove(me_member);
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