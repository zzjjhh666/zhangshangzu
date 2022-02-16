using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;

namespace HPIT.RentHouse.lService
{
    public interface IPermissionsService : IServiceSupport
    {
        List<PermissionDTO> GetList();
        List<PermissionDTO> GetPageList(int start, int length, string name, ref int count);
        AjaxResult Add(PermissionDTO permission);
        PermissionDTO Edit(long id);
        AjaxResult Edit(PermissionDTO permission);
        AjaxResult Delete(long id);
        AjaxResult DeleteBatch(List<long> ids);
    }
}
