using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPIT.RentHouse.lService;

namespace HPIT.RentHouse.Service
{
    public class RolesService : IRolesService
    {
        public List<RolesDTO> GetList()
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Roles>(db);
            var list = bs.GetList(e => true).Select(e => new RolesDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return list;
        }

        public List<RolesDTO> GetPageList(int start, int length, string name, ref int count)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Roles>(db);
            var query = PredicateExtensions.True<T_Roles>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.And(e => e.Name.Contains(name));
            }
            var list = bs.GetPagedList(start, length, ref count, query, a => a.Id);
            var result = list.Select(a => new RolesDTO
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
            return result;
        }
        public AjaxResult Add(RolesDTO roles)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Roles>(db);
            var bsp = new BaseService<T_Permissions>(db);
            T_Roles role = new T_Roles();
            role.Name = roles.Name;
            role.CreateDateTime = DateTime.Now;
            if (roles.PermissionIds!=null&&roles.PermissionIds.Count>0)
            {
                foreach (var ids in roles.PermissionIds)
                {
                    var per = bsp.Get(a => a.Id == ids);
                    role.T_Permissions.Add(per);
                }
            }
            long id = bs.Add(role);
            if (id > 0)
            {
                return new AjaxResult(ResultState.Success, "角色添加成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "角色添加失败");
            }
        }
        public RolesDTO Edit(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_Roles> bs = new BaseService<T_Roles>(db);
            BaseService<T_Permissions> bsp = new BaseService<T_Permissions>(db);
            T_Roles model = bs.Get(a => a.Id == id);
            RolesDTO dto = new RolesDTO();
            if (model != null)
            {
                dto.Name = model.Name;
                dto.PermissionIds = model.T_Permissions.Select(t => t.Id).ToList();
            }
            return dto;
        }
        public AjaxResult Edit(RolesDTO roles)
        {
            var db = new RentHouseEntity();
            BaseService<T_Roles> bs = new BaseService<T_Roles>(db);
            BaseService<T_Permissions> bsp = new BaseService<T_Permissions>(db);
            T_Roles role = new T_Roles();
            var model = bs.Get(a => a.Id == roles.Id);
            model.Name = roles.Name;
            model.T_Permissions.Clear();
            if (roles.PermissionIds != null && roles.PermissionIds.Count > 0)
            {
                foreach (var ids in roles.PermissionIds)
                {
                    var per = bsp.Get(a => a.Id == ids);
                    model.T_Permissions.Add(per);
                }
            }
            bool res = bs.Update(model);
            if (res)
            {
                return new AjaxResult(ResultState.Success, "角色修改成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "角色修改失败");
            }
        }
        public AjaxResult Delete(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_Roles> bs = new BaseService<T_Roles>(db);
            var model = bs.Get(b => b.Id == id);
            if (bs.Delete(model))
            {
                return new AjaxResult(ResultState.Success, "角色删除成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "角色删除失败");
            }
        }

        public AjaxResult DeleteBatch(List<long> ids)
        {
            try
            {
                var db = new RentHouseEntity();
                BaseService<T_Roles> bs = new BaseService<T_Roles>(db);
                foreach (var id in ids)
                {
                    var model = bs.Get(b => b.Id == id);
                    bs.Delete(model);
                }
                return new AjaxResult(ResultState.Success, "角色删除成功");
            }
            catch (Exception)
            {

                return new AjaxResult(ResultState.Error, "角色删除失败");
            }

        }
    }
}
