using Apps.BLL.Sys;
using Apps.Common;
using Apps.DAL;
using Apps.Models.Sys;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace Apps.WebApi.Controllers
{
    /// <summary>  
    /// 客户信息  
    /// </summary>  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : WebAPI2BaseController
    {
        private SysUserBLL user_BLL = new SysUserBLL();
        log4net.ILog log = log4net.LogManager.GetLogger("GaiFix.Logging");

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        [HttpGet]
        public HttpResponseMessage sendCode(string mobile)
        {
            var response = new Response();

            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes(Constant.APP_SECRET + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");

            WebRequest request = WebRequest.Create("http://api.sms.ronghub.com/sendCode.json");
            request.Method = "POST";
            string postData = "mobile=" + mobile + "&templateId=" + Constant.TEMPLATE_REGISTER + "&region=86";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("App-Key", Constant.APP_KEY);
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse resp = request.GetResponse();
            dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ReturnJson _ReturnJson = jsonSerializer.Deserialize<ReturnJson>(responseFromServer);
            var code = _ReturnJson.code;
            var sessionId = _ReturnJson.sessionId;
            if (code == 200)
            {
                response.Code = 0;
                response.Message = "发送验证码成功！";
                context.Cache.Insert(mobile, sessionId);
            }
            else
            {
                response.Code = 1;
                response.Message = "发送验证码失败！";
                context.Cache.Insert(mobile, "");
            }
            reader.Close();
            dataStream.Close();
            resp.Close();

            response.Data = null;
            return toJson(response);
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        [HttpGet]
        public HttpResponseMessage verifyCode(string mobile, string code)
        {
            var response = new Response();

            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes(Constant.APP_SECRET + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");

            WebRequest request = WebRequest.Create("http://api.sms.ronghub.com/verifyCode.json");
            request.Method = "POST";
            string postData = "sessionId=" + context.Cache[mobile] + "&code=" + code;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("App-Key", Constant.APP_KEY);
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse resp = request.GetResponse();
            dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ReturnJson _ReturnJson = jsonSerializer.Deserialize<ReturnJson>(responseFromServer);
            var rtnCode = _ReturnJson.code;
            var rtnSuccess = _ReturnJson.success;
            if (rtnCode == 200 && "true".ToUpper().Equals(rtnSuccess.ToUpper()))
            {
                response.Code = 0;
                response.Message = "手机号验证成功！";
            }
            else
            {
                response.Code = 1;
                response.Message = "手机号验证失败！";
            }
            reader.Close();
            dataStream.Close();
            resp.Close();

            response.Data = rtnSuccess;
            return toJson(response);
        }

        /// <summary>
        /// 外线员登陆
        /// </summary>
        /// <param name="EmployeeNo">员工号</param>
        /// <param name="Password">密码</param>
        [HttpGet]
        public HttpResponseMessage login(string EmployeeNo, string Password)
        {
            log.Debug("OutsideTroubleManController.login() Start!");
            log.Debug("EmployeeNo is " + EmployeeNo + ", Password is " + Password);
            var response = new Response();
            response.Code = 0;
            response.Message = "外线员登陆成功！";
            SysUser user = user_BLL.login(EmployeeNo, Password);
            if (user.Id == 0)
            {
                response.Code = 1;
                response.Message = "员工号或者密码错误！";
                response.Data = null;
            }
            else
            {
                response.Data = user;
            }
            return toJson(response);
        }

        /// <summary>
        /// 获取外线员详情
        /// </summary>
        /// <param name="OTM_ID">外线员ID</param>
        [HttpGet]
        public HttpResponseMessage GetOTM(int OTM_ID)
        {
            log.Debug("OutsideTroubleManController.GetOTM() Start!");
            log.Debug("OTM_ID is " + OTM_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取外线员详情成功！";
            SysUser user = user_BLL.m_Rep.Find(Convert.ToInt32(OTM_ID));
            if (user.Id == 0)
            {
                response.Code = 1;
                response.Message = "获取外线员详情失败！";
                response.Data = null;
            }
            else
            {
                response.Data = user;
            }
            return toJson(response);
        }
    }
}