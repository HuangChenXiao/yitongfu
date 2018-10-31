using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Com
{
    public static class JwtHelper
    {
        public static JwtModel setToken(JwtModel md)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Convert.ToInt32(Math.Round((now - unixEpoch).TotalSeconds));
            secondsSinceEpoch +=24 * 60 * 60;
            md.exp = secondsSinceEpoch;
            var payload = new Dictionary<string, object>
                {
                    { "id", md.userid },
                    { "usercode", md.rolecode },
                    { "username", md.username },
                    { "isadmin", md.isadmin },
                    { "rolecode", md.rolecode },
                    { "exp", md.exp }
                };
            var secret = "9720cbfbb0684617a2afbe466e100ba2";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            md.token = encoder.Encode(payload, secret);
            md.message = "获取成功";
            md.status_code = 200;
            return md;
        }
        public static JwtModel getToken(string token)
        {
            JwtModel jwtmodel = new JwtModel();
            var secret = "9720cbfbb0684617a2afbe466e100ba2";
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                var decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new[] { new DynamicJsonConverter() });

                dynamic result = serializer.Deserialize<object>(json);
                jwtmodel.userid = result.id;
                jwtmodel.usercode = result.usercode;
                jwtmodel.username = result.username;
                jwtmodel.isadmin = result.isadmin;
                jwtmodel.rolecode = result.rolecode;
                jwtmodel.message = "获取成功";
                jwtmodel.status_code = 200;
            }
            catch (TokenExpiredException)
            {
                jwtmodel.message = "Token has expired";
                jwtmodel.status_code = 402;
            }
            catch (SignatureVerificationException)
            {
                jwtmodel.message = "Token has invalid signature";
                jwtmodel.status_code = 402;
            }
            catch (Exception)
            {
                jwtmodel.message = "Token has invalid signature";
                jwtmodel.status_code = 402;
            }
            return jwtmodel;
        }
    }

}
