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

namespace WebAPI.Controllers.LifeCircle
{
    public class QrCodeController : ApiController
    {
        private ytfEntities db = new ytfEntities();
        JsonModel model = new JsonModel();
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_qrcode
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


        // PUT: api/Admin/5
        [ResponseType(typeof(sy_qrcode))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Put(sy_qrcode sy_qrcode)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                //var count = db.sy_qrcode.Where(o => o. == sy_qrcode.usercode && o.userid != sy_qrcode.userid).Count();
                //if (count > 0)
                //{
                //    model.message = "用户编码已经存在";
                //    model.status_code = 401;
                //}
                //else
                //{
                db.Entry(sy_qrcode).State = EntityState.Modified;
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
                //}
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }

        // POST: api/sy_qrcode
        [ResponseType(typeof(sy_qrcode))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Postsy_qrcode(sy_qrcode sy_qrcode)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                //var info = db.sy_qrcode.Where(o => o.usercode == sy_qrcode.usercode).Count();
                //if (info > 0)
                //{
                //    model.message = "用户编码已经存在";
                //    model.status_code = 401;
                //}
                //else
                //{
                sy_qrcode.addtime = DateTime.Now;
                db.sy_qrcode.Add(sy_qrcode);
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
                //}
            }
            else
            {
                model.message = "用户权限不足";
                model.status_code = 401;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        [ResponseType(typeof(sy_qrcode))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Delete(int id)
        {
            sy_qrcode sy_qrcode = db.sy_qrcode.Find(id);
            if (sy_qrcode == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.sy_qrcode.Remove(sy_qrcode);
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