using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.lService
{
    public interface IAdminUsersService : IServiceSupport
    {
        List<AdminUsersDTO> GetList();
        List<AdminUsersDTO> GetPageList(int start, int length, string name, ref int count);
        AjaxResult Add(AdminUsersDTO admin);
        AdminUsersDTO Edit(long id);
        AjaxResult Edit(AdminUsersDTO admin);
        List<CitiesDTO> CityList();
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AjaxResult Delete(long id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        AjaxResult DeleteBatch(List<long> ids);
    }
}
