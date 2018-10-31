using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebAPI.Controllers
{
    [WebApiActionDebugFilter]
    public class UploadController : ApiController
    {
        [HttpPost]
        public ResponseMessageResult Get()
        {
            Dictionary<string, object> path = new Dictionary<string, object>();
            try
            {
                //HttpPostedFile file = HttpContext.Current.Request.Files.Count;
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string FilePathName = HttpContext.Current.Server.MapPath("../UpLoad/");
                string S = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

                if (file.ContentLength > 0)
                {
                    if (!Directory.Exists(FilePathName))//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(FilePathName);
                    }
                    //获得保存路径
                    string filePath = Path.Combine(FilePathName,
                                    Path.GetFileName(S));
                    file.SaveAs(filePath);
                    path.Add("message", "上传成功");
                    path.Add("status_code", 200);
                    path.Add("url", "http://" + HttpContext.Current.Request.Url.Authority + "/Upload/" + S);
                }
            }
            catch (Exception ex)
            {
                path.Add("message", ex.Message);
                path.Add("status_code", 401);
                return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)401, path));
            }
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)200, path));
        }
    }
}
