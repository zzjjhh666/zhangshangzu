using HPIT.RentHouse.Admin.Filters;
using HPIT.RentHouse.Admin.Models;
using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
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
            ViewBag.TypeList = _idNameServcie.GetIdNameList(IdNameEnum.房屋类型).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.StatusList = _idNameServcie.GetIdNameList(IdNameEnum.房屋状态).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.DecorateStatusList = _idNameServcie.GetIdNameList(IdNameEnum.装修状态).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            ViewBag.RoomTypeList = _idNameServcie.GetIdNameList(IdNameEnum.户型).Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() }).ToList();
            var list = _housesService.GetAttachmentList();
            return View(list);
        }
        /// <summary>
        /// 提交添加房源表单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add_Per(HousesAddDTO dto)
        {
            var result = _housesService.Add(dto);
            return Json(result);
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
        /// <summary>
        /// 编辑房源信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long id)
        {

            //获取当前登录管理员的信息，根据当前管理所管辖的地区，来获取对应的区域
            var currentAdmin = User as MyFormsPrincipal<LoginAdminDTO>;

            //根据CityId获取当前所在的区域
            ViewBag.RegionList = _regionService.GetRegionList(currentAdmin.UserData.CityId).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            //房型
            ViewBag.RoomTypeList = _idNameServcie.GetIdNameList(IdNameEnum.户型).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            //装修类型
            ViewBag.DecorateStatusList = _idNameServcie.GetIdNameList(IdNameEnum.装修状态).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            //状态
            ViewBag.StatusList = _idNameServcie.GetIdNameList(IdNameEnum.房屋状态).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            //类型
            ViewBag.TypeList = _idNameServcie.GetIdNameList(IdNameEnum.房屋类型).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            //房源设施
            ViewBag.AttachmentList = _housesService.GetAttachmentList();


            var dto = _housesService.GetHouse(id);

            //获取当前房源所在区域的小区
            ViewBag.CommunityList = _communityService.GetCommunities(dto.RegionId).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            return View(dto);
        }

        /// <summary>
        /// 提交编辑房源信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(HousesEditDTO dto)
        {
            var result = _housesService.EditHouse(dto);
            return Json(result);
        }
    }
}