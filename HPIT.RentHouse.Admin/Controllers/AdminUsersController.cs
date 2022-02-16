using HPIT.RentHouse.Admin.Models;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.RentHouse.Admin.Controllers
{
    [Authorize]
    public class AdminUsersController : Controller
    {
        // GET: AdminUsers
        private IRolesService _rolesService;
        private IAdminUsersService _adminUsersService;
        public AdminUsersController(IAdminUsersService adminUsersService, IRolesService rolesService)
        {
            _rolesService = rolesService;
            _adminUsersService = adminUsersService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int start, int length, string name)
        {
            int count = 0;
            var list = _adminUsersService.GetPageList(start, length, name, ref count);
            PageModel pageModel = new PageModel();
            pageModel.data = list;
            pageModel.recordsTotal = count;
            pageModel.recordsFiltered = count;
            return Json(pageModel);
        }
        public ActionResult Add()
        {
            var rolList = _rolesService.GetList();
            ViewBag.rolList = rolList;
            ViewBag.CityList = _adminUsersService.CityList();
            return View();
        }
        [HttpPost]
        public ActionResult Add_AdminUser(AdminUsersDTO admin)
        {
            var add = _adminUsersService.Add(admin);
            return Json(add);
        }
        public ActionResult Edit(long id)
        {
            AdminUsersDTO dto = _adminUsersService.Edit(id);
            var rolList = _rolesService.GetList();
            ViewBag.rolList = rolList;
            ViewBag.CityList = _adminUsersService.CityList();
            return View(dto);
        }
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="adminUsers"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit_Per(AdminUsersDTO adminUsers)
        {

            var ad = _adminUsersService.Edit(adminUsers);
            return Json(ad);
        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var result = _adminUsersService.Delete(id);
            return Json(result);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBatch(List<long> ids)
        {
            var result = _adminUsersService.DeleteBatch(ids);
            return Json(result);
        }
    }
}