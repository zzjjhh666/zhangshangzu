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
    public class IdNameServcie : IIdNameServcie
    {
        public static List<IdNameDTO> idNameList;

        public IdNameServcie()
        {
            using (var db = new RentHouseEntity())
            {
                var bs = new BaseService<T_IdNames>(db);
                idNameList = bs.GetList(e => true).Select(e => new IdNameDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    TypeName = e.TypeName
                }).ToList();
            }
        }

        public List<IdNameDTO> GetIdNameList(IdNameEnum typeName)
        {
            var list = idNameList.Where(e => e.TypeName == typeName.ToString()).ToList();
            return list;
        }

        public string GetName(long id)
        {
            var name = idNameList.Where(e => e.Id == id).FirstOrDefault().Name;
            return name;
        }
    }
}
