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
    public class RouterController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();

        JsonModel model = new JsonModel();
        public ResponseMessageResult Getsy_router(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_router
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
        /// <summary>
        /// 查询路由拼接
        /// </summary>
        /// <returns></returns>
        public ResponseMessageResult Getsy_router()
        {
            List<Dictionary<string, object>> arr = new List<Dictionary<string, object>>();
            Dictionary<string, object> main;
            List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();
            Dictionary<string, object> detail;
            var router = db.sy_router.OrderByDescending(o => o.sort).ToList();
            var routers = db.sy_routers.OrderByDescending(o => o.sort).ToList();
            foreach (var item in router)
            {
                main = new Dictionary<string, object>();
                main.Add("name", item.name);
                main.Add("icon", item.icon);
                main.Add("id", item.id);
                main.Add("meta", item.meta);
                main.Add("noDropdown", item.nodropdown);
                main.Add("path", item.path);
                main.Add("redirect", item.redirect);
                main.Add("sort", item.sort);
                main.Add("hidden", item.hidden);

                children = new List<Dictionary<string, object>>();
                foreach (var dtl in routers)
                {
                    if (item.id == dtl.id)
                    {
                        detail = new Dictionary<string, object>();
                        detail.Add("name", dtl.name);
                        detail.Add("meta", dtl.meta);
                        detail.Add("path", dtl.path);
                        detail.Add("sort", dtl.sort);
                        detail.Add("component", dtl.component);
                        detail.Add("autoid", dtl.autoid);
                        detail.Add("hidden", dtl.hidden);
                        children.Add(detail);
                    }
                }
                if (children.Count > 0)
                {
                    main.Add("children", children);
                }
                arr.Add(main);
            }
            model.message = "查询成功";
            model.status_code = 200;
            model.data = arr;
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
        // PUT: api/Admin/5
        [ResponseType(typeof(sy_router))]
        public ResponseMessageResult Putsy_router(sy_router sy_router)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                db.Entry(sy_router).State = EntityState.Modified;
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

        // POST: api/sy_router
        [ResponseType(typeof(sy_router))]
        public ResponseMessageResult Postsy_router(sy_router sy_router)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                db.sy_router.Add(sy_router);
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
        [ResponseType(typeof(sy_router))]
        public ResponseMessageResult Deletesy_router(int id)
        {
            sy_router sy_router = db.sy_router.Find(id);
            if (sy_router == null)
            {
                model.message = "删除失败";
                model.status_code = 401;
            }

            db.sy_router.Remove(sy_router);
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