using HPIT.RentHouse.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.lService
{
    public interface IHousesService : IServiceSupport
    {
        /// <summary>
        /// 获取房源信息
        /// </summary>
        /// <param name="communityName"></param>
        /// <returns></returns>
        List<HousesDTO> GetHouseList(string communityName, int typeId, int start, int length, ref int totalCount);
        /// <summary>
        /// 获取房源附属设施
        /// </summary>
        /// <returns></returns>
        List<AttachmentDTO> GetAttachmentList();
    }
}
