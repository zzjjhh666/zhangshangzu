using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPIT.RentHouse.Admin.Models
{
    public class PageModel
    {
        public object data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

    }
}