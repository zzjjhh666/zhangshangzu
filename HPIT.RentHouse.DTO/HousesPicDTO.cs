using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class HousesPicDTO
    {
        public long Id { get; set; }
        public long HouseId { get; set; }
        public string Url { get; set; }
        public string ThumbUrl { get; set; }
    }
}
