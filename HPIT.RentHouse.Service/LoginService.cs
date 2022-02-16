using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using HPIT.RentHouse.Service.Entities;

namespace HPIT.RentHouse.Service
{
    public class LoginService : ILoginService
    {
        public AjaxResult Login(LoginDTO dto)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_AdminUsers>(db);
                var user = bs.Get(a => a.PhoneNum == dto.PhoneNum);
                //根据手机号判断用户是否存在
                if (user == null)
                {
                    return new AjaxResult(ResultState.Error, "用户不存在");
                }
                else
                {
                    //判断密码是否正确
                    var salt = user.PasswordSalt;
                    var pwd = CommonHelper.CalcMD5(dto.Password + salt);
                    if (pwd != user.PasswordHash)
                    {
                        return new AjaxResult(ResultState.Error, "密码错误");
                    }
                    else
                    {
                        //缓存用户信息
                        var admindto = new LoginAdminDTO();
                        admindto.CityId = user.CityId == null ? 0 : (long)user.CityId;
                        admindto.Id = user.Id;
                        admindto.Name = user.Name;
                        admindto.PhoneNum = user.PhoneNum;
                        admindto.RoleName = user.T_Roles.Count > 0 ? user.T_Roles.FirstOrDefault().Name : "";
                        SaverData(admindto, dto.IsRemeberMe);

                        return new AjaxResult(ResultState.Success, "登录成功");
                    }
                }
            }
        }
        //缓存用户信息
        public void SaverData(LoginAdminDTO user, bool isRemeberMe)
        {
            //1、序列化要保存的用户信息
            var data = user.ToJson();

            //2、创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, user.Name, DateTime.Now, DateTime.Now.AddDays(1), true, data);

            //3、加密保存
            string cookieValue = FormsAuthentication.Encrypt(ticket);

            // 4. 根据加密结果创建登录Cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            //若用户勾选记得我，则指定cookie有效期（ Expires ）
            if (isRemeberMe)
            {
                cookie.Expires = DateTime.Now.AddDays(1);
            }

            // 5. 写登录Cookie（获取http请求上下文HttpContext ，移除原来的数据，存入新的）
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
