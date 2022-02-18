using HPIT.RentHouse.Admin.Filters;
using HPIT.RentHouse.Admin.Models;
using HPIT.RentHouse.Common;
using HPIT.RentHouse.IService;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.RentHouse.Admin.Controllers
{
    [Authorize]
    public class HousesController : Controller
    {
        // GET: Houses
        private readonly IHousesService _housesService;
        private readonly IRegionService _regionService;
        private readonly IIdNameServcie _idNameServcie;
        private readonly ICommunityService _communityService;
        public HousesController(IHousesService housesService, IRegionService regionService,
                        IIdNameServcie idNameServcie, ICommunityService communityService)
        {
            _housesService = housesService;
            _regionService = regionService;
            _idNameServcie = idNameServcie;
            _communityService = communityService;
        }
        /// <summary>
        /// 加载房源列表
        /// </summary>
        /// <returns></returns>
        //[CheckMyPermission("lookHouse")]
        public ActionResult Index(int typeId)
        {
            ViewBag.typeId = typeId;
            return View();
        }

        /// <summary>
        /// 获取房源分页数据
        /// </summary>
        /// <param name="communityName"></param>
        /// <param name="typeId"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetHouseList(string communityName, int typeId, int start, int length)
        {
            int count = 0;
            var list = _housesService.GetHouseList(communityName, typeId, start, length, ref count);
            PageModel pageModel = new PageModel();
            pageModel.data = list;
            pageModel.recordsTotal = count;
            pageModel.recordsFiltered = count;
            return Json(pageModel);
        }
        /// <summary>
        /// 添加房源
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {

            ViewBag.RegionList = _regionService.GetRegionList(0).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.RoomTypeList = _idNameServcie.GetIdNameList(IdNameEnum.房屋类型).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.StatusList = _idNameServcie.GetIdNameList(IdNameEnum.房屋状态).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.DecorateStatusList = _idNameServcie.GetIdNameList(IdNameEnum.装修状态).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.TypeList = _idNameServcie.GetIdNameList(IdNameEnum.户型).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var list = _housesService.GetAttachmentList();
            return View(list);
        }

        /// <summary>
        /// 根据区域id获取小区信息
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCommunities(int regionId)
        {
            var list = _communityService.GetCommunities(regionId);
            return Json(list);
        }
    }
}