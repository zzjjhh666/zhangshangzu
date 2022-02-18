using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.IService
{
    public interface ICommunityService : IServiceSupport
    {
        /// <summary>
        /// 根据区域Id获取小区
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        List<CommuityDTO> GetCommunities(long regionId);
    }
}
