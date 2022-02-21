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

        public AjaxResult Add(HousesAddDTO dto)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Houses>(db);
            T_Houses model = new T_Houses();
            model.Address = dto.Address;
            model.Area = dto.Area;
            model.CheckInDateTime = dto.CheckInDateTime;
            model.CommunityId = dto.CommunityId;
            model.DecorateStatusId = dto.DecorateStatusId;
            model.Description = dto.Description;
            model.Direction = dto.Direction;
            model.FloorIndex = dto.FloorIndex;
            model.LookableDateTime = dto.LookableDateTime;
            model.MonthRent = dto.MonthRent;
            model.OwnerName = dto.OwnerName;
            model.OwnerPhoneNum = dto.OwnerPhoneNum;
            model.RoomTypeId = dto.RoomTypeId;
            model.StatusId = dto.StatusId;
            model.TotalFloorCount = dto.TotalFloorCount;
            model.TypeId = dto.TypeId;
            model.IsDeleted = dto.IsDeleted;
            model.CreateDateTime = DateTime.Now;
            long id = bs.Add(model);
            if (id > 0)
            {
                return new AjaxResult(ResultState.Success, "房源添加成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "房源添加失败");
            }
        }
        /// <summary>
        /// 根据id获取房源信息
        /// </summary>
        /// <param name="id">房源id</param>
        /// <returns></returns>
        public HousesEditDTO GetHouse(long id)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Houses>(db);

                var model = bs.Get(e => e.Id == id);
                if (model != null)
                {
                    var dto = new HousesEditDTO()
                    {
                        Address = model.Address,
                        Area = model.Area,
                        CheckInDateTime = model.CheckInDateTime,
                        CityId = model.T_Communities.T_Regions.CityId,
                        CommunityId = model.CommunityId,
                        DecorateStatusId = model.DecorateStatusId,
                        Description = model.Description,
                        Direction = model.Direction,
                        FloorIndex = model.FloorIndex,
                        Id = model.Id,
                        IsDeleted = model.IsDeleted,
                        LookableDateTime = model.LookableDateTime,
                        MonthRent = model.MonthRent,
                        OwnerName = model.OwnerName,
                        OwnerPhoneNum = model.OwnerPhoneNum,
                        RegionId = model.T_Communities.RegionId,
                        RoomTypeId = model.RoomTypeId,
                        StatusId = model.StatusId,
                        TotalFloorCount = model.TotalFloorCount,
                        TypeId = model.TypeId,

                    };

                    //获取所有配置设置信息
                    var bsAttachment = new BaseService<T_Attachments>(db);

                    var attachmentList = bsAttachment.GetList(e => true).ToList();

                    dto.HouseAttachments = new List<HouseAttachment>();

                    //获取当前房源的配置设置信息
                    var currentAttachment = model.T_Attachments;

                    attachmentList.ForEach(e =>
                    {
                        var houseAttachment = new HouseAttachment()
                        {
                            AttachmentId = e.Id,
                            Name = e.Name,
                            IsUsed = currentAttachment.Any(c => c.Id == e.Id)
                        };

                        dto.HouseAttachments.Add(houseAttachment);
                    });

                    return dto;
                }
                return null;
            }
        }
        /// <summary>
        /// 更新房源
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public AjaxResult EditHouse(HousesEditDTO dto)
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_Houses>(db);

                var model = bs.Get(e => e.Id == dto.Id);
                if (model != null)
                {
                    model.Address = dto.Address;
                    model.Area = dto.Area;
                    model.CheckInDateTime = dto.CheckInDateTime;
                    model.CommunityId = dto.CommunityId;
                    model.DecorateStatusId = dto.DecorateStatusId;
                    model.Description = dto.Description;
                    model.Direction = dto.Direction;
                    model.FloorIndex = dto.FloorIndex;
                    model.LookableDateTime = dto.LookableDateTime;
                    model.MonthRent = dto.MonthRent;
                    model.OwnerName = dto.OwnerName;
                    model.OwnerPhoneNum = dto.OwnerPhoneNum;
                    model.RoomTypeId = dto.RoomTypeId;
                    model.StatusId = dto.StatusId;
                    model.TotalFloorCount = dto.TotalFloorCount;
                    model.TypeId = dto.TypeId;

                    //清空当前房源的配置设置信息
                    model.T_Attachments.Clear();

                    //添加新的配置设置信息

                    var bsAttachment = new BaseService<T_Attachments>(db);
                    if (dto.AttachmentIds != null && dto.AttachmentIds.Length > 0)
                    {
                        for (int i = 0; i < dto.AttachmentIds.Length; i++)
                        {
                            var attachmentId = dto.AttachmentIds[i];

                            var attachment = bsAttachment.Get(e => e.Id == attachmentId);
                            if (attachment != null)
                            {
                                model.T_Attachments.Add(attachment);
                            }

                        }
                    }

                    db.SaveChanges();

                    return new AjaxResult(ResultState.Success, "修改成功");

                }
                return new AjaxResult(ResultState.Error, "当前房源信息不存在");
            }
        }
    }
}
