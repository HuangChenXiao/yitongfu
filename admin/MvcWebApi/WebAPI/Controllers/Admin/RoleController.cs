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
using Com;
using System.Web;
using WebAPI.Models;
using System.Web.Http.Results;

namespace WebAPI.Controllers.Admin
{
    [WebApiActionDebugFilter]
    public class RoleController : ApiController
    {
        private ytfEntities db = new ytfEntities();
        JsonModel model = new JsonModel();

        // GET: api/Role
        public ResponseMessageResult Getsy_role()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_role
                           select new
                               {
                                   a.rolename,
                                   a.roleid,
                                   a.rolecode,
                                   a.isadmin,
                                   a.addtime,
                                   a.adduser,
                                   a.updatetime,
                                   a.updateuser
                               };
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

        // PUT: api/Role/5
        [ResponseType(typeof(void))]
        public ResponseMessageResult Putsy_role(sy_role sy_role)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                sy_role.updatetime = DateTime.Now;
                sy_role.updateuser = jwtmodel.username;
                db.Entry(sy_role).State = EntityState.Modified;
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

        // POST: api/Role
        [ResponseType(typeof(sy_role))]
        public IHttpActionResult Postsy_role(sy_role sy_role)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_role.Where(o => o.rolecode == sy_role.rolecode).Count();
                if (info > 0)
                {
                    model.message = "角色编码已经存在";
                    model.status_code = 401;
                }
                else
                {
                    sy_role.addtime = DateTime.Now;
                    sy_role.adduser = jwtmodel.username;
                    db.sy_role.Add(sy_role);
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

        private bool sy_roleExists(int id)
        {
            return db.sy_role.Count(e => e.roleid == id) > 0;
        }
    }
}