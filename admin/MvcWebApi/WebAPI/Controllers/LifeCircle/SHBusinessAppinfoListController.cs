using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class SHBusinessAppinfoListController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(int status, int page, int pagesize, string keyword = "")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_SHBusinessAppinfoList @0,@1,@2,@3", keyword, status, page, pagesize).ToList();

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
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(int sh_businessid, decimal amount)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.QuerySingle("exec sh_business_addbalance @0,@1", sh_businessid, amount);

                if (query.status_code == 1)
                {
                    model.message = query.message;
                    model.status_code = 200;
                    model.data = query;
                }
                else
                {
                    model.message = query.message;
                    model.status_code = 401;
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
        [ResponseType(typeof(sh_business_appinfo))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Putsh_business_appinfo(sh_business_appinfo sh_business_appinfo)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var count = db.sh_business_appinfo.Where(o => o.sh_appid == sh_business_appinfo.sh_appid && o.id != sh_business_appinfo.id).Count();
                if (count > 0)
                {
                    model.message = "appid已经存在";
                    model.status_code = 401;
                }
                else
                {
                    //db.Entry(sh_business_appinfo).State = EntityState.Modified;
                    var info = db.sh_business_appinfo.Find(sh_business_appinfo.id);
                    if (!string.IsNullOrEmpty(sh_business_appinfo.sh_password))
                    {
                        info.sh_password = BaseHelper.Md5Hash(sh_business_appinfo.sh_password);
                    }
                    info.sh_appid = sh_business_appinfo.sh_appid;
                    info.sh_appsecret = sh_business_appinfo.sh_appsecret;
                    info.sh_storeid = sh_business_appinfo.sh_storeid;
                    info.sh_storename = sh_business_appinfo.sh_storename;
                    info.sh_businessname = sh_business_appinfo.sh_businessname;
                    info.sh_mobile = sh_business_appinfo.sh_mobile;
                    info.sh_balance = sh_business_appinfo.sh_balance;
                    info.sh_commission = sh_business_appinfo.sh_commission;
                    info.sh_commratio = sh_business_appinfo.sh_commratio;
                    info.agid = sh_business_appinfo.agid;
                    info.status = sh_business_appinfo.status;
                    info.isused = sh_business_appinfo.isused;
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

        // POST: api/sh_business_appinfo
        [ResponseType(typeof(sh_business_appinfo))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Post(sh_business_appinfo sh_business_appinfo)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var info = db.sh_business_appinfo.Where(o => o.sh_appid == sh_business_appinfo.sh_appid).Count();
                if (info > 0)
                {
                    model.message = "appid已经存在";
                    model.status_code = 401;
                }
                else
                {
                    db.sh_business_appinfo.Add(sh_business_appinfo);
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

    }
}
