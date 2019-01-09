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

namespace WebAPI.Controllers.Agent
{
    public class AgentMerchantController : ApiController
    {

        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        /// <summary>
        /// 获取代理商的商户
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(sy_agent))]
        [WebApiActionDebugFilter]
        public ResponseMessageResult Get()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            var sy_merchant = from a in db.sy_merchant
                              select new { 
                                id=a.id.ToString(),
                                a.name,
                              };
            if (sy_merchant == null)
            {
                model.message = "暂无数据";
                model.status_code = 200;
            }
            else
            {
                model.data = sy_merchant.ToList();
                model.message = "查询成功";
                model.status_code = 200;
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
