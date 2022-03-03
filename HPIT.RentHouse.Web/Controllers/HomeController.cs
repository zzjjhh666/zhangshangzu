using HPIT.RentHouse.Common;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.RentHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminUsersService _adminUsersService;
        private readonly IHousesService _housesService;
        public HomeController(IAdminUsersService adminUsersService, IHousesService housesService)
        {
            _adminUsersService = adminUsersService;
            _housesService = housesService;
        }
        public ActionResult Index(int cityId = 0)
        {
            var list = _adminUsersService.CityList();
            ViewBag.CityList = list;
            if (cityId > 0)
            {
                ViewBag.cityId = cityId;
                ViewBag.DefaultCity = list.Where(e => e.Id == cityId).FirstOrDefault().Name;
            }
            else
            {
                ViewBag.cityId = list.FirstOrDefault().Id;
                ViewBag.DefaultCity = list.FirstOrDefault().Name;
            }
            return View();
        }
        [HttpPost]
        public ActionResult LoadMore(int pageIndex, int cityId)
        {
            var list = _housesService.GetList(cityId, pageIndex, 6);
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.FirstThumbUrl))
                {
                    item.FirstThumbUrl = CommonHelper.GetServerIP() + item.FirstThumbUrl;
                }
            }
            return Json(list);
        }
        public ActionResult Entire() 
        {
            return View();
        }
    }
}