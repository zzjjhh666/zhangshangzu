using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class HousesEditDTO:HousesAddDTO
    {
        public long Id { get; set; }

        public List<HouseAttachment> HouseAttachments { get; set; }
    }
    public class HouseAttachment
    {
        public long AttachmentId { get; set; }

        public string Name { get; set; }

        public bool IsUsed { get; set; }
    }
}
