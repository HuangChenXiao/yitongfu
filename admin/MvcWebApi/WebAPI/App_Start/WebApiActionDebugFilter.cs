using Com;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using WebAPI.Models;

namespace WebAPI
{
    public class WebApiActionDebugFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>  
        /// 是否开启调试  
        /// </summary>  
        private bool _isDebugLog = true;
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext context)
        {
            if (_isDebugLog)
            {
                var Authorization = context.Request.Headers.Authorization == null ? string.Empty : context.Request.Headers.Authorization.ToString();
                JwtModel model = JwtHelper.getToken(Authorization);
                if (model.status_code != 200)
                {
                    context.Response = new HttpResponseMessage
                    {
                        Content = new StringContent("{\"message\":\"" + model.message + "\",\"status_code\":" + model.status_code + "}",
                            Encoding.GetEncoding("UTF-8"), "application/json"),
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                    return;
                }
            }
            base.OnActionExecuting(context);
        }

    }
}