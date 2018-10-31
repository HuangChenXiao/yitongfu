using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com
{
    public class JwtModel
    {
        public int userid { get; set; }
        public string usercode { get; set; }
        public string username { get; set; }
        public bool isadmin { get; set; }
        public int exp { get; set; }
        public string rolecode { get; set; }
        public string appid { get; set; }
        public string appsecret { get; set; }
        public string message { get; set; }
        public int status_code { get; set; }
        public string token { get; set; }
    }
}
