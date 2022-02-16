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
    public class AdminUsersService : IAdminUsersService
    {
        public List<AdminUsersDTO> GetList()
        {
            throw new NotImplementedException();
        }

        public List<AdminUsersDTO> GetPageList(int start, int length, string name, ref int count)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_AdminUsers>(db);
            var query = PredicateExtensions.True<T_AdminUsers>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.And(e => e.Name.Contains(name));
            }
            var list = bs.GetPagedList(start, length, ref count, query, a => a.Id);
            var result = list.Select(a => new AdminUsersDTO
            {
                Id = a.Id,
                Name = a.Name,
                PhoneNum = a.PhoneNum,
                Email = a.Email,
                LoginErrorTimes = a.LoginErrorTimes,
                CityId = a.T_Cities.Name
            }).ToList();
            return result;
        }
        public AjaxResult Add(AdminUsersDTO admin)
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_AdminUsers>(db);
            var bsr = new BaseService<T_Roles>(db);
            T_AdminUsers users = new T_AdminUsers()
            {
                Id = admin.Id,
                Name = admin.Name,
                PhoneNum = admin.PhoneNum,
                PasswordHash = admin.PasswordHash,
                PasswordSalt = admin.PasswordSalt,
                Email = admin.Email,
                LoginErrorTimes = admin.LoginErrorTimes,
                LastLoginErrorDateTime = DateTime.Now,
                CityId = Convert.ToInt32(admin.CityId),
                CreateDateTime = DateTime.Now
            };
            if (admin.RolesIds != null && admin.RolesIds.Count > 0)
            {
                foreach (var ids in admin.RolesIds)
                {
                    var per = bsr.Get(a => a.Id == ids);
                    users.T_Roles.Add(per);
                }
            }
            long id = bs.Add(users);
            if (id > 0)
            {
                return new AjaxResult(ResultState.Success, "管理员添加成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "管理员添加失败");
            }
        }
        /// <summary>
        /// 获取城市表字段
        /// </summary>
        /// <returns></returns>
        public List<CitiesDTO> CityList()
        {
            var db = new RentHouseEntity();
            var bs = new BaseService<T_Cities>(db);
            var list = bs.GetList(e => true).Select(e => new CitiesDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return list;
        }
        public AdminUsersDTO Edit(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_AdminUsers> bs = new BaseService<T_AdminUsers>(db);
            T_AdminUsers model = bs.Get(a => a.Id == id);
            AdminUsersDTO dto = new AdminUsersDTO();
            if (model != null)
            {
                dto.Name = model.Name;
                dto.CityId = model.CityId.ToString();
                dto.Email = model.Email;
                dto.LoginErrorTimes = model.LoginErrorTimes;
                dto.PasswordHash = model.PasswordHash;
                dto.PasswordSalt = model.PasswordSalt;
                dto.PhoneNum = model.PhoneNum;
                dto.LastLoginErrorDateTime = DateTime.Now;
                dto.RolesIds = model.T_Roles.Select(r => r.Id).ToList();
            }
            return dto;
        }

        public AjaxResult Edit(AdminUsersDTO adminUsers)
        {
            var db = new RentHouseEntity();
            BaseService<T_AdminUsers> bs = new BaseService<T_AdminUsers>(db);
            var model = bs.Get(a => a.Id == adminUsers.Id);
            var bsa = new BaseService<T_Roles>(db);
            model.Name = adminUsers.Name;
            model.LoginErrorTimes = adminUsers.LoginErrorTimes;
            //model.PasswordSalt = adminUsers.PasswordSalt;
            //model.PasswordHash = adminUsers.PasswordHash;
            model.PasswordSalt = CommonHelper.CreateVerifyCode(5);
            model.PasswordHash = CommonHelper.CalcMD5(adminUsers.PasswordHash + model.PasswordSalt);
            model.PhoneNum = adminUsers.PhoneNum;
            model.LastLoginErrorDateTime = DateTime.Now;
            model.CityId = Convert.ToInt32(adminUsers.CityId);
            model.Email = adminUsers.Email;
            model.T_Roles.Clear();
            if (adminUsers.RolesIds != null && adminUsers.RolesIds.Count > 0)
            {
                foreach (var ids in adminUsers.RolesIds)
                {
                    T_Roles roles = bsa.Get(p => p.Id == ids);
                    model.T_Roles.Add(roles);
                }
            }
            bool res = bs.Update(model);
            if (res)
            {
                return new AjaxResult(ResultState.Success, "管理员修改成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "管理员修改失败");
            }

        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AjaxResult Delete(long id)
        {
            var db = new RentHouseEntity();
            BaseService<T_AdminUsers> bs = new BaseService<T_AdminUsers>(db);
            var model = bs.Get(a => a.Id == id);
            if (bs.Delete(model))
            {
                return new AjaxResult(ResultState.Success, "管理员删除成功");
            }
            else
            {
                return new AjaxResult(ResultState.Error, "管理员删除失败");
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public AjaxResult DeleteBatch(List<long> ids)
        {
            try
            {
                var db = new RentHouseEntity();
                BaseService<T_AdminUsers> bs = new BaseService<T_AdminUsers>(db);
                foreach (var id in ids)
                {
                    var model = bs.Get(a => a.Id == id);
                    bs.Delete(model);
                }
                return new AjaxResult(ResultState.Success, "管理员删除成功");
            }
            catch (Exception)
            {
                return new AjaxResult(ResultState.Error, "管理员删除失败");
            }
        }

    }
}
