using Com;
using ModelDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Export
{
    /// <summary>
    /// PutForwardOrder 的摘要说明
    /// </summary>
    public class PutForwardOrder : IHttpHandler
    {
        JsonModel model = new JsonModel();
        public void ProcessRequest(HttpContext context)
        {
            var db = ContextDB.Context();
            var begindate = context.Request["begindate"];
            var enddate = context.Request["enddate"];
            var agid = context.Request["agid"];
            var merchantid = context.Request["merchantid"];
            var status = context.Request["status"];
            var businesspasstype = context.Request["businesspasstype"];
            var page = 1;
            var pagesize = 10000000;
            var query = db.Query("exec PROC_DespoitOrderList @0,@1,@2,@3,@4,@5,@6,@7", begindate, enddate, 0, 0, status, page, pagesize, businesspasstype).ToList();

            DataTable table = new DataTable();
            if (query.Count > 0)
            {
                model.message = "查询成功";
                model.status_code = 200;
                model.data = query;
                table.Columns.Add("订单号", typeof(string));
                table.Columns.Add("订单金额", typeof(string));
                table.Columns.Add("订单状态", typeof(string));
                table.Columns.Add("代理商", typeof(string));
                table.Columns.Add("商户名称", typeof(string));
                table.Columns.Add("银行卡号", typeof(string));
                table.Columns.Add("开卡人", typeof(string));
                table.Columns.Add("手机号", typeof(string));
                table.Columns.Add("原因", typeof(string));
                table.Columns.Add("订单时间", typeof(string));
                foreach (var item in query)
                {
                    int it_status=item.status;
                    string str_status = "";
                    switch (it_status)
                    {
                        case 0:
                            str_status = "提现中";
                            break;
                        case 1:
                            str_status = "代理商审核";
                            break;
                        case 2:
                            str_status = "总部审核";
                            break;
                        case 3:
                            str_status = "审核不通过";
                            break;
                        case 4:
                            str_status = "代付成功中";
                            break;
                        case 5:
                            str_status = "代付成功";
                            break;
                        case 6:
                            str_status = "代付失败";
                            break;
                    }
                    table.Rows.Add(item.despoitno, item.amount, str_status, item.agname,  item.customername,  item.bankaccountno, item.bankusername, item.mobileno, item.reason, item.addtime);
                }
            }
            else
            {
                model.message = "暂无数据";
                model.status_code = 200;
                context.Response.Write("查询不到导出数据");
            }
            db.Dispose();
            ExcelRender.RenderToExcel(table, context, "订单列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}