using HPIT.RentHouse.Admin.Models;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using HPIT.RentHouse.Service;
using HPIT.RentHouse.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.RentHouse.Admin.Controllers
{
    [Authorize]
    public class PermissionsController : Controller
    {
        private IPermissionsService _permissionsService;
        public PermissionsController(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }
        //public ActionResult Index()
        //{
        //    IPermissionService permissionsService = new PermissionService();
        //    return View();
        //}
        public ActionResult Index()
        {
            //IPermissionService permissionsService = new PermissionService();
            var list = _permissionsService.GetList();
            return View();
        }
        public ActionResult GetList(int start, int length, string name)
        {
            //int start = 0;
            //int length = 12;
            //string name = "";
            int count = 0;
            var list = _permissionsService.GetPageList(start, length, name, ref count);
            PageModel pageModel = new PageModel();
            pageModel.data = list;
            pageModel.recordsTotal = count;
            pageModel.recordsFiltered = count;
            return Json(pageModel);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add_Per(PermissionDTO permission)
        {
            var ad = _permissionsService.Add(permission);
            return Json(ad);
        }
        public ActionResult Edit(long id)
        {
            PermissionDTO dto = _permissionsService.Edit(id);
            return View(dto);
        }
        [HttpPost]
        public ActionResult Edit_Per(PermissionDTO dto)
        {
            var ad = _permissionsService.Edit(dto);
            return Json(ad);
        }
        public ActionResult Delete(long id) 
        {
            var res=_permissionsService.Delete(id);
            return Json(res);
        }
        public ActionResult DeleteBatch(List<long> ids) 
        {
            var res = _permissionsService.DeleteBatch(ids);
            return Json(res);
        }
    }
}