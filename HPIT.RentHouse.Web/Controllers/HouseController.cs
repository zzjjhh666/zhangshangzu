using HPIT.RentHouse.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;

namespace HPIT.RentHouse.Web.Controllers
{
    public class HouseController : Controller
    {
        private readonly IHousesService _houseService;
        private readonly IAdminUsersService _adminUsersService;
        private readonly IRegionService _regionService;

        public HouseController(IHousesService houseService,IAdminUsersService adminUsersService, IRegionService regionService)
        {
            _houseService = houseService;
            _adminUsersService = adminUsersService;
            _regionService = regionService;
        }
        /// <summary>
        /// 房源搜索
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(HouseSearchOptions house) 
        {
            var cityList = _adminUsersService.CityList();
            house.CityId = cityList.FirstOrDefault().Id;

            ViewBag.DefaultCity = cityList.FirstOrDefault().Name;
            ViewBag.RegionList = _regionService.GetRegionList(cityList.FirstOrDefault().Id);
            //月租条件处理
            if (!string.IsNullOrWhiteSpace(house.MonthRent))
            {
                var strMonthRent = house.MonthRent.Split('-');

                if (strMonthRent[0] != "*")
                {
                    house.StartMonthRent = Convert.ToInt32(strMonthRent[0]);
                }

                if (strMonthRent[1] != "*")
                {
                    house.EndMonthRent = Convert.ToInt32(strMonthRent[1]);
                }
            }
            var list = _houseService.Search(house);
            return View(list);
        }



















        //public ActionResult Detail(int id)
        //{
        //    var model = _houseService.GetHouseDetail(id);
        //    return View(model);
        //}
    }
}