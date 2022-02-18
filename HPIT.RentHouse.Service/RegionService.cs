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
    public class RegionService : IRegionService
    {
        /// <summary>
        /// 根据城市Id获取该城市所有的区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<RegionDTO> GetRegionList(long cityId)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Regions>(db);
                var query = PredicateExtensions.True<T_Regions>();
                if (cityId > 0)
                {
                    query = query.And(e => e.CityId == cityId);
                }
                var list = bs.GetList(query).Select(e => new RegionDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    CityId = e.CityId
                }).ToList(); ;
                return list;
            }
        }
    }
}
