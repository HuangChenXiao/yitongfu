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

namespace WebAPI.Controllers.Merchant
{
    public class GetMerchantCardController : ApiController
    {
        private ytfEntities db = new ytfEntities();

        JsonModel model = new JsonModel();
        /// <summary>
        /// 获取虚拟商户银行卡信息
        /// </summary>
        /// <returns></returns>
        [WebApiActionDebugFilter]
        public ResponseMessageResult Getfa_business_basic()
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var temp = from a in db.fa_business_basic
                           join b in db.fa_business_bank_card on a.id equals b.businessid
                           where a.id == jwtmodel.userid
                           select new 
                           {
                               b.id,
                               card_bankaccountNo=b.bankaccountNo,
                               a.appId
                           };
                model.total = temp.Count();
                model.data = temp.ToList();

                if (model.data.Count>0)
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
