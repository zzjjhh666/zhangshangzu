using HPIT.RentHouse.Common;
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
        /// <summary>
        /// 添加房源
        /// </summary>
        /// <param name="houses"></param>
        /// <returns></returns>
        AjaxResult Add(HousesAddDTO houses);
        /// <summary>
        /// 根据id获取房源信息
        /// </summary>
        /// <param name="id">房源id</param>
        /// <returns></returns>
        HousesEditDTO GetHouse(long id);

        /// <summary>
        /// 更新房源信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        AjaxResult EditHouse(HousesEditDTO dto);
    }
}
