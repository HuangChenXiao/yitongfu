using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class OrderExportController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult GetOrderList(int status, int page, int pagesize, string keyword, string begindate = "", string enddate = "")
        {
            JwtModel jwtmodel = JwtHelper.getToken(HttpContext.Current.Request.Headers.GetValues("Authorization").First().ToString());
            if (jwtmodel.isadmin)
            {
                var db = ContextDB.Context();
                var query = db.Query("exec PROC_OrderList @0,@1,@2,@3,@4,@5,@6,@7", begindate, enddate, 0, 0, keyword, status, page, pagesize).ToList();

                if (query.Count > 0)
                {
                    model.message = "查询成功";
                    model.status_code = 200;
                    model.data = query;

                    DataTable table = new DataTable();
                    table.Columns.Add("aa", typeof(string));
                    table.Columns.Add("bb", typeof(string));
                    table.Columns.Add("cc", typeof(string));
                    for (int i = 0; i < 10; i++)
                    {
                        string a = DateTime.Now.Ticks.ToString();
                        Thread.Sleep(1);
                        string b = DateTime.Now.Ticks.ToString();
                        Thread.Sleep(1);
                        string c = DateTime.Now.Ticks.ToString();
                        Thread.Sleep(1);
                        table.Rows.Add(a, b, c);
                    }

                    ExcelRender.RenderToExcel(table, HttpContext.Current.Request.Context, "看看.xls");
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
    }
}
