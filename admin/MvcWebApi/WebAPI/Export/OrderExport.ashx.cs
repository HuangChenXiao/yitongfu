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
    /// OrderExport 的摘要说明
    /// </summary>
    public class OrderExport : IHttpHandler
    {
        JsonModel model = new JsonModel();
        public void ProcessRequest(HttpContext context)
        {
            var db = ContextDB.Context();
            var begindate = context.Request["begindate"];
            var enddate = context.Request["enddate"];
            var agid = context.Request["agid"];
            var merchantid = context.Request["merchantid"];
            var keyword = context.Request["keyword"];
            var status = context.Request["status"];
            var businesspasstype = context.Request["businesspasstype"];
            var page = 1;
            var pagesize =10000000;
            var query = db.Query("exec PROC_OrderList @0,@1,@2,@3,@4,@5,@6,@7,@8", begindate, enddate, agid, merchantid, keyword, status, page, pagesize, businesspasstype).ToList();

            DataTable table = new DataTable();
            if (query.Count > 0)
            {
                model.message = "查询成功";
                model.status_code = 200;
                model.data = query;	
                table.Columns.Add("订单号", typeof(string));
                table.Columns.Add("订单金额", typeof(string));
                table.Columns.Add("手续费", typeof(string));
                table.Columns.Add("实到金额", typeof(string));
                table.Columns.Add("订单状态", typeof(string));
                table.Columns.Add("代理商", typeof(string));
                table.Columns.Add("真实商户", typeof(string));
                table.Columns.Add("付款类型", typeof(string));
                table.Columns.Add("备注", typeof(string));
                table.Columns.Add("订单时间", typeof(string));
                foreach (var item in query)
                {
                    var item_status = item.status == "1" ? "支付完成" : item.status == "0" ? "未支付" : "支付失败";
                    var item_paytype = item.status == "1" ? "微信" : item.status == "2" ? "支付宝" : item.status == "3" ? "QQ钱包":"京东";
                    table.Rows.Add(item.orderno, item.amount, item.servicecharge, item.realamount, item_status,
                                    item.agname, item.shortname, item_paytype, item.remark, item.addtime);
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