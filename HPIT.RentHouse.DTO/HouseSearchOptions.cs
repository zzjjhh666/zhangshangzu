using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class HouseSearchOptions
    {
        public long CityId { get; set; }

        public string Keywords { get; set; }
        public long RegionId { get; set; }
        public string MonthRent { get; set; }

        public int? StartMonthRent { get; set; }
        public int? EndMonthRent { get; set; }

        public string OrderByType { get; set; }
    }
}
