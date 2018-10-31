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

namespace WebAPI.Controllers
{
    public class AdminController : ApiController
    {
        private ytf_dbEntities db = new ytf_dbEntities();
        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_admin(int page, int pagesize, string code)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.sy_admin
                           join b in db.sy_role on a.roleid equals b.roleid
                           where (a.username.Contains(code) || a.usercode.Contains(code) || string.IsNullOrEmpty(code))
                           && b.isadmin == false
                           select new
                           {
                               a.userid,
                               a.usercode,
                               a.username,
                               a.roleid,
                               b.rolecode,
                               b.rolename,
                               a.status,
                               b.isadmin,
                               a.addtime,
                               a.adduser,
                               a.updatetime,
                               a.updateuser
                           };
                model.total = temp.Count();
                model.data = temp.OrderByDescending(s => s.userid).Skip((page - 1) * pagesize).Take(pagesize).ToList();

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
        // GET: api/Admin 用户登录
        public ResponseMessageResult Getsy_admin(string code, string password)
        {
            var temp = from a in db.sy_admin
                       join b in db.sy_role on a.roleid equals b.roleid
                       select new
                       {
                           a.userid,
                           a.usercode,
                           a.username,
                           a.roleid,
                           b.rolecode,
                           b.rolename,
                           a.status,
                           b.isadmin,
                           a.password,
                           a.addtime,
                           a.adduser,
                           a.updatetime,
                           a.updateuser
                       };
            password = BaseHelper.Md5Hash(password);
            model.data = temp.Where(o => o.usercode == code && o.password == password && o.status == 1).FirstOrDefault();
            if (model.data != null)
            {
                JwtModel jwtmodel = new JwtModel();
                jwtmodel.userid = model.data.userid;
                jwtmodel.usercode = model.data.usercode;
                jwtmodel.username = model.data.username;
                jwtmodel.isadmin = model.data.isadmin;
                jwtmodel.rolecode = model.data.rolecode;
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

        // GET: api/Admin/5
        [ResponseType(typeof(sy_admin))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getsy_admin()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var sy_admin = from a in db.sy_admin
                               join b in db.sy_role on a.roleid equals b.roleid
                               where a.userid == jwtmodel.userid
                               select new
                               {
                                   a.userid,
                                   a.usercode,
                                   a.username,
                                   a.roleid,
                                   b.rolecode,
                                   b.rolename,
                                   a.status,
                                   b.isadmin,
                                   a.addtime,
                                   a.adduser,
                                   a.updatetime,
                                   a.updateuser
                               };
                if (sy_admin == null)
                {
                    model.message = "暂无数据";
                    model.status_code = 200;
                }
                else
                {
                    model.data = sy_admin.FirstOrDefault();
                    model.message = "查询成功";
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
        [ResponseType(typeof(sy_admin))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Putsy_admin(sy_admin sy_admin)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var count = db.sy_admin.Where(o => o.usercode == sy_admin.usercode && o.userid!=sy_admin.userid).Count();
                if (count > 0)
                {
                    model.message = "用户编码已经存在";
                    model.status_code = 401;
                }
                else
                {
                    var info = db.sy_admin.Find(sy_admin.userid);
                    if (!string.IsNullOrEmpty(sy_admin.password))
                    {
                        info.password = BaseHelper.Md5Hash(sy_admin.password);
                    }
                    info.username = sy_admin.username;
                    info.usercode = sy_admin.usercode;
                    info.status = sy_admin.status;
                    info.roleid = sy_admin.roleid;
                    info.updatetime = DateTime.Now;
                    info.updateuser = jwtmodel.username;
                    //db.Entry(sy_admin).State = EntityState.Modified;
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

        // POST: api/sy_admin
        [ResponseType(typeof(sy_admin))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Postsy_admin(sy_admin sy_admin)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sy_admin.Where(o => o.usercode == sy_admin.usercode).Count();
                if (info > 0)
                {
                    model.message = "用户编码已经存在";
                    model.status_code = 401;
                }
                else
                {
                    sy_admin.addtime = DateTime.Now;
                    sy_admin.adduser = jwtmodel.username;
                    db.sy_admin.Add(sy_admin);
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