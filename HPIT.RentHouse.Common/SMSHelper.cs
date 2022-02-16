using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.Common
{
    public class SMSHelper
    {
        // 请根据实际 accesskey 和 secretkey 进行开发，以下只作为演示 sdk 使用
        static string accesskey = "5f0fb728efb9a35396a2ce9a";
        static string secretkey = "e6d633417d8c4d69a99e21a0c0dadb18";
        //同时支持http和https两种协议，具体使用情况根据自己需求选择。
        static string url = "https://live.kewail.com/sms/v1/sendsinglesms";
        // static String url = "http://127.0.0.1:8080/live.kewail.com/sms/v1/sendsinglesms";

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string SendSMS(string mobile)
        {
            long capata = getRandom();
            string msg = "【Kewail科技】 您的验证码" + capata + "工作人员不会索取，请勿泄漏。";

            // 手机号码
            string phoneNumber = mobile;
            // 普通单发,注意前面必须为【】符号包含，置于头或者尾部。
            string singleSenderResult = send(0, "86", phoneNumber, msg, "", "");
            return singleSenderResult;
        }

        /// <summary>
        /// 普通单发短信接口，明确指定内容，如果有多个签名，请在内容中以【】的方式添加到信息内容中，否则系统将使用默认签名
        /// </summary>
        /// <param name="type">短信类型，0 为普通短信，1 营销短信</param>
        ///<param name="nationCode">国家码，如 86 为中国</param>
        /// <param name="phoneNumber">不带国家码的手机号</param>
        /// <param name="signId">短信签名的Id</param>
        /// <param name="templateId">短信模板Id</param>
        /// <param name="param">,短信的变量参数值 ，将短信模板中的变量{0},{1}替换为参数中的值，如果短信模板中没有变量，这这个值填null</param>
        /// <param name="extend">扩展码，可填空</param>
        /// <param name="ext">服务端原样返回的参数，可填空</param>
        /// <returns>返回发送结果字符串</returns>
        private static String send(int type, String nationCode, String phoneNumber, String msg, String extend, String ext)
        {
            // 校验 type 类型
            if (0 != type && 1 != type)
            {
                throw new Exception("type " + type + " error");
            }
            if (null == extend)
            {
                extend = "";
            }
            if (null == ext)
            {
                ext = "";
            }
            long random = getRandom();
            long curTime = getCurTime();

            string wholeUrl = url + "?accesskey=" + accesskey + "&random=" + random;

            Console.WriteLine(wholeUrl);

            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(wholeUrl);


            string sig = "secretkey=" + secretkey + "&random=" + random + "&time=" + curTime + "&mobile=" + phoneNumber;
            sig = SHA256Encrypt(sig);


            Encoding encoding = new UTF8Encoding();


            // String tel = "{"nationcode": + nationCode ,"mobile":"" + phoneNumber+""}";
            // string postData = "{"type": + type+ ,"msg": + msg+ ,"sig": + sig+ ,"tel": + tel+ ,"time": + curTime+ ,"extend": + extend+ ,"ext": + ext+""}";

            JsonTel tel = new JsonTel();
            JsonData postData = new JsonData();
            tel.nationcode = nationCode;
            tel.mobile = phoneNumber;

            postData.type = type;
            postData.msg = msg;
            postData.sig = sig;
            postData.tel = tel;
            postData.time = curTime;
            postData.extend = extend;
            postData.ext = ext;

            string json = JsonConvert.SerializeObject(postData);

            Console.WriteLine(json);


            byte[] data = encoding.GetBytes(json);



            httpWReq.ProtocolVersion = HttpVersion.Version11;
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";//charset=UTF-8";
            httpWReq.Headers.Add("X-Amzn-Type-Version",
                                               "com.amazon.device.messaging.ADMMessage@1.0");
            httpWReq.Headers.Add("X-Amzn-Accept-Type",
                                            "com.amazon.device.messaging.ADMSendResult@1.0");
            // httpWReq.Headers.Add(HttpRequestHeader.Authorization,
            //"Bearer " + accessToken);
            httpWReq.ContentLength = data.Length;


            Stream stream = httpWReq.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            string s = response.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonresponse = "";
            string temp = null;
            while ((temp = reader.ReadLine()) != null)
            {
                jsonresponse += temp;
            }
            return jsonresponse;

        }

        public static long getRandom()
        {
            Random random = new Random();
            return (random.Next(999999)) % 900000 + 100000;
        }

        /// <summary>    
        /// 获取时间戳    
        /// </summary>  
        public static long getCurTime()
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //  return Convert.ToInt64(ts.TotalSeconds).ToString();
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000;
        }

        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            //System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            //byte[] byte1;
            //byte1 = s256.ComputeHash(Encoding.UTF8.GetBytes(str));
            //s256.Clear();
            //return Convert.ToBase64String(byte1);

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            HashAlgorithm algorithm = null;
            algorithm = new SHA256Managed();
            return BitConverter.ToString(algorithm.ComputeHash(bytes)).Replace("-", "").ToLower();
        }

        public class JsonData
        {
            public int type

            {
                get;
                set;
            }

            public string sig

            {
                get;
                set;
            }



            public long time

            {
                get;
                set;
            }

            public string ext

            {
                get;
                set;
            }


            public string msg

            {
                get;
                set;
            }

            public string extend

            {
                get;
                set;
            }

            public JsonTel tel

            {
                get;
                set;
            }


        }

        public class JsonTel
        {

            public string nationcode

            {
                get;
                set;
            }

            public string mobile

            {
                get;
                set;
            }
        }
    }
}
