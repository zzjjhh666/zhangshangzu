using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class CommuityDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long RegionId { get; set; }

        public string Location { get; set; }

        public string Traffic { get; set; }

        public int BuiltYear { get; set; }
    }
}
