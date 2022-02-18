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
    public class PermissionsService : IPermissionsService
    {
        public List<PermissionDTO> GetList()
        {
            var db = new RentHouseEntity();
            BaseService<T_Permissions> bs = new BaseService<T_Permissions>(db);
            var list = bs.GetList(e => true).Select(e => new PermissionDTO
            {
                Description = e.Description,
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return list;
        }
        public List<PermissionDTO> GetPageList(int start, int length, string name, ref int count)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Permissions>(db);
            var query = PredicateExtensions.True<T_Permissions>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.And(e => e.Name.Contains(name));
            }
            var list = bs.GetPagedList(start, length, ref count, query, a => a.Id);
            var result = list.Select(a => new PermissionDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();
            return result;
        }
        public AjaxResult Add(PermissionDTO permission)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Permissions>(db);
            T_Permissions permissions = new T_Permissions();
            permissions.Name = permission.Name;
            permissions.Description = permission.Description;
            permissions.CreateDateTime = DateTime.Now;
            long id = bs.Add(permissions);
            if (id > 0)
            {
                return new AjaxResult(ResultState.Success, "管理员添加成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "管理员添加失败");
            }
        }
        public PermissionDTO Edit(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_Permissions> bs = new BaseService<T_Permissions>(db);
            T_Permissions model = bs.Get(a => a.Id == id);
            PermissionDTO dto = new PermissionDTO();
            if (model != null)
            {
                dto.Name = model.Name;
                dto.Description = model.Description;
            }
            return dto;
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public AjaxResult Edit(PermissionDTO permission)
        {
            var db = new RentHouseEntity();
            BaseService<T_Permissions> bs = new BaseService<T_Permissions>(db);
            var model = bs.Get(a => a.Id == permission.Id);
            model.Name = permission.Name;
            model.Description = permission.Description;
            bool res = bs.Update(model);
            if (res)
            {
                return new AjaxResult(ResultState.Success, "权限修改成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "权限修改失败");
            }
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AjaxResult Delete(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_Permissions> bs = new BaseService<T_Permissions>(db);
            var model = bs.Get(b => b.Id == id);
            if (bs.Delete(model)) 
            {
               return new AjaxResult(ResultState.Success, "权限删除成功");
            }
            else
            {
               return new AjaxResult(ResultState.Error, "权限删除失败");
            }
        }
        /// <summary>
        /// 批量删除权限
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public AjaxResult DeleteBatch(List<long> ids)
        {
            try
            {
                var db = new RentHouseEntity();
                BaseService<T_Permissions> bs = new BaseService<T_Permissions>(db);
                foreach (var id in ids)
                {
                    var model = bs.Get(b => b.Id == id);
                    bs.Delete(model);
                }
                return new AjaxResult(ResultState.Success, "权限删除成功");
            }
            catch (Exception)
            {

                return new AjaxResult(ResultState.Error, "权限删除失败");
            }
            
        }
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<PermissionDTO> GetListByUser(long UserId)
        {
            var db = new RentHouseEntity();
            BaseService<T_AdminUsers> bs = new BaseService<T_AdminUsers>(db);
            var user = bs.Get(b => b.Id == UserId);
            List<T_Roles> roleList = user.T_Roles.ToList();
            List<PermissionDTO> permissionList = new List<PermissionDTO>();
            foreach (var role in roleList)
            {
                var result = role.T_Permissions.Select(a => new PermissionDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                });
                permissionList.AddRange(result);
            }
            return permissionList;
        }
    }
}
