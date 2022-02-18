using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.IService
{
    public interface IRegionService : IServiceSupport
    {
        /// <summary>
        /// 根据城市Id获取该城市所有的区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<RegionDTO> GetRegionList(long cityId);
    }
}
