using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.lService
{
    public interface IRolesService : IServiceSupport
    {
        List<RolesDTO> GetList();
        List<RolesDTO> GetPageList(int start, int length, string name, ref int count);
        AjaxResult Add(RolesDTO roles);
        RolesDTO Edit(long id);
        AjaxResult Edit(RolesDTO roles);
        AjaxResult Delete(long id);
        AjaxResult DeleteBatch(List<long> ids);
    }
}
