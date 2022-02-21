using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    public class HousesAddDTO
    {
        public bool IsDeleted { get; set; }
        public long CityId { get; set; }
        public long RegionId { get; set; }
        public string Address { get; set; }
        public int MonthRent { get; set; }
        public long StatusId { get; set; }
        public decimal Area { get; set; }
        public long DecorateStatusId { get; set; }
        public int TotalFloorCount { get; set; }
        public int FloorIndex { get; set; }
        public long RoomTypeId { get; set; }
        public long TypeId { get; set; }
        public long CommunityId { get; set; }
        public DateTime LookableDateTime { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNum { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }

        /// <summary>
        /// 配套设施
        /// </summary>
        public long[] AttachmentIds { get; set; }
    }
}
