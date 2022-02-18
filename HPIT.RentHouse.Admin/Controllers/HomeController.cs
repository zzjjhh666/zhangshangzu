using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HPIT.RentHouse.lService;
using CaptchaGen;

namespace HPIT.RentHouse.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IAdminUsersService _adminUsersService;
        public HomeController(IAdminUsersService adminUsersService) 
        {
            _adminUsersService = adminUsersService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login() 
        {
            return View();
        }
        public ActionResult GetVerCode()
        {
            //1、生成验证码文字——调用CommonHelper中随时生成验证码文字的方法
            string code = CommonHelper.CreateVerifyCode(4);

            //2、使用TempData临时存储验证码文字
            TempData["VerCode"] = code;

            //3、使用CaptchaGen实现验证码生成
            var src = ImageFactory.GenerateImage(code, 60, 100, 15, 6);

            //4、返回文件
            return File(src, "image/jpeg");

        }

        [HttpPost]
        public ActionResult SubmitLogin(LoginDTO dto)
        {
            if (TempData["VerCode"] == null)
            {
                return Json(new AjaxResult(ResultState.Error, "验证码已失效"));
            }
            else
            {
                string code = TempData["VerCode"].ToString();
                if (code != dto.VerCode)
                {
                    return Json(new AjaxResult(ResultState.Error, "验证码输入有误"));
                }
                else
                {
                    var result = _adminUsersService.Login(dto);
                    return Json(result);
                }
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}