using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.DTO
{
    [Serializable]
    public class HousesDTO
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 小区名
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 地段
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 租金
        /// </summary>
        public int MonthRent { get; set; }

        /// <summary>
        /// 房间类型
        /// </summary>
        public string RoomTypeName { get; set; }

        /// <summary>
        /// 装修类型
        /// </summary>
        public string DecorateStatusName { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public decimal Area { get; set; }

        public long RoomTypeId { get; set; }

        public long DecorateStatusId { get; set; }

        public string FirstThumbUrl { get; set; }
    }
}

