using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.Common
{
    /// <summary>
    /// json序列化扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 将对象序列化为json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            //进行序列化 1.系统自带的序列化方法  2.json.net 中  3. redis组件中
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将json格式字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
               return JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
                return default(T);
            }
        }
    }
}
