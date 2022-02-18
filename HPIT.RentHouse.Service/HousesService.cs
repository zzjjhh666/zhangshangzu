using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using HPIT.RentHouse.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.Service
{
    public class HousesService : IHousesService
    {
        public List<HousesDTO> GetHouseList(string communityName, int typeId, int start, int length, ref int totalCount)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Houses>(db);
                var query = PredicateExtensions.True<T_Houses>();
                if (typeId > 0)
                {
                    query = query.And(e => e.TypeId == typeId);
                }
                if (!string.IsNullOrWhiteSpace(communityName))
                {
                    query = query.And(e => e.T_Communities.Name.Contains(communityName));
                }
                totalCount = 0;
                var list = bs.GetPagedList(start, length, ref totalCount, query, e => e.Id, false).Select(e => new HousesDTO
                {
                    Address = e.Address,
                    Area = e.Area,
                    CommunityName = e.T_Communities.Name,
                    DecorateStatusId = e.DecorateStatusId,
                    MonthRent = e.MonthRent,
                    Id = e.Id,
                    RoomTypeId = e.RoomTypeId,
                    RegionName = e.T_Communities.T_Regions.Name
                }).ToList();
                var bsIdNames = new BaseService<T_IdNames>(db);
                var idNamesList = bsIdNames.GetList(e => true).ToList();
                list.ForEach(e =>
                {
                    e.DecorateStatusName = idNamesList.Where(i => i.Id == e.DecorateStatusId).FirstOrDefault().Name;
                    e.RoomTypeName = idNamesList.Where(i => i.Id == e.RoomTypeId).FirstOrDefault().Name;
                });
                return list;
            }
        }
        public List<AttachmentDTO> GetAttachmentList()
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Attachments>(db);
                var list = bs.GetList(e => true).Select(e => new AttachmentDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    IconName = e.IconName
                }).ToList(); ;
                return list;
            }
        }
    }
}
