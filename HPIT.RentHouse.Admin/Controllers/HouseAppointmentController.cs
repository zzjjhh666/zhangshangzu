using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPIT.RentHouse.Admin.Controllers
{
    [Authorize]
    public class HouseAppointmentController : Controller
    {
        // GET: HouseAppointment
        public ActionResult Index()
        {
            return View();
        }
    }
}