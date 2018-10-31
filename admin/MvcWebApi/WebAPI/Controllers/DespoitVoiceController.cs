using ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class DespoitVoiceController : ApiController
    {
        JsonModel model = new JsonModel();
        public ResponseMessageResult Get(int agid)
        {
            var db = ContextDB.Context();
            var query = db.QuerySingle("exec PROC_DespoitVoice @0", agid);
            if (query.status_code == 1)
            {
                model.message = "1";
                model.status_code = 200;
            }
            else
            {
                model.message = "0";
                model.status_code = 200;
            }

            db.Dispose();
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)model.status_code, model));
        }
    }
}
