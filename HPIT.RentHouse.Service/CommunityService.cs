using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.IService;
using HPIT.RentHouse.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.Service
{
    public class CommunityService : ICommunityService
    {
        /// <summary>
        /// 根据区域Id获取小区
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public List<CommuityDTO> GetCommunities(long regionId)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Communities>(db);
                var query = PredicateExtensions.True<T_Communities>();
                if (regionId > 0)
                {
                    query = query.And(e => e.RegionId == regionId);
                }
                var list = bs.GetList(query).Select(e => new CommuityDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    BuiltYear = e.BuiltYear == null ? 0 : (int)e.BuiltYear,
                    Location = e.Location,
                    RegionId = e.RegionId,
                    Traffic = e.Traffic
                }).ToList(); ;
                return list;
            }

        }
    }
}
