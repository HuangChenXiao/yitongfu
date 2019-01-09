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
    public class BusBalanceController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        // GET: api/Admin 用户列表
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get(int page, int pagesize, string code, int agentid,int businessid=0)
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.aa_business_balance
                           join b in db.fa_business_basic on a.businessid equals b.id
                           where (b.shortName.Contains(code) || string.IsNullOrEmpty(code))
                           && (b.agentid == agentid || agentid == 0)
                           && (b.id == businessid || businessid == 0)
                           select new
                           {
                               a.id,
                               a.businessid,
                               a.businesspasstype,
                               a.wechatbalance,
                               a.alipaybalance,
                               a.unionpaybalance,
                               a.totalbalance,
                               b.shortName
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
    }
}
