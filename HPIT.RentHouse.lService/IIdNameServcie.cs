using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.IService
{
    public interface IIdNameServcie : IServiceSupport
    {
        /// <summary>
        /// 根据类型名获取数据字典集合
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        List<IdNameDTO> GetIdNameList(IdNameEnum typeName);

        /// <summary>
        /// 根据Id获取对应的名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetName(long id);
    }
}
