using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class RegionDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CityId { get; set; }
    }
}
