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
    public class AgentRatioController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        public ResponseMessageResult Getsy_agentratio(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_agentratio
                           join b in db.sy_agent on a.agid equals b.id
                           where (b.name.Contains(code) || string.IsNullOrEmpty(code))
                           select new
                           {
                               a.id,
                               a.agid,
                               a.businesspasstype,
                               a.ratio,
                               b.name
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
        [ResponseType(typeof(sy_agentratio))]
        public ResponseMessageResult Putsy_agentratio(sy_agentratio sy_agentratio)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_agentratio.Where(o => o.agid == sy_agentratio.agid && o.id != sy_agentratio.id && o.businesspasstype == sy_agentratio.businesspasstype).Count();
                if (info > 0)
                {
                    model.message = "已存在通道类型！";
                    model.status_code = 401;
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
                }
                db.Entry(sy_agentratio).State = EntityState.Modified;
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

        // POST: api/sy_agentratio
        [ResponseType(typeof(sy_agentratio))]
        public ResponseMessageResult Postsy_agentratio(sy_agentratio sy_agentratio)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_agentratio.Where(o => o.agid == sy_agentratio.agid && o.businesspasstype == sy_agentratio.businesspasstype).Count();
                if (info > 0)
                {
                    model.message = "已存在通道类型！";
                    model.status_code = 401;
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
                }
                db.sy_agentratio.Add(sy_agentratio);
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
        [ResponseType(typeof(sy_agentratio))]
        public ResponseMessageResult Deletesy_agentratio(int id)
        {
            sy_agentratio sy_agentratio = db.sy_agentratio.Find(id);
            if (sy_agentratio == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.sy_agentratio.Remove(sy_agentratio);
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