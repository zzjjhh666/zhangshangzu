using HPIT.RentHouse.Common;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HPIT.RentHouse.Admin.Filters
{
    /// <summary>
    /// 授权过滤器——基于用户授权和基于角色授权
    /// </summary>
    public class CheckMyPermissionAttribute : AuthorizeAttribute
    {
        public string permissionName { get; set; }
        public CheckMyPermissionAttribute(string name)
        {
            this.permissionName = name;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1、获取该用户信息,得到id
            var user = filterContext.HttpContext.User as MyFormsPrincipal<LoginAdminDTO>;
            long userId = user.UserData.Id;

            //2、获取当前登录的用户权限（根据登录用户的id查询所对应的权限信息）
            //【注】在过滤器中调用Services时，由于当前类不是Autofac创建的，所以不会自动进行注入，所以需要手动获取Service：DependencyResolver.Current.GetService<T>()
            IPermissionsService permissionsService = DependencyResolver.Current.GetService<IPermissionsService>();
            List<PermissionDTO> userPermissions = permissionsService.GetListByUser(userId);//当前用户权限

            //3、判断当前的请求权限是否在用户角色所拥有的权限中，没有则做处理
            if (!userPermissions.Any(a => a.Name == permissionName))
            {
                var msg = $"{user.UserData.Name}没有权限{permissionName}";
                //如果执行操作类是ajax请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var res = new AjaxResult(ResultState.Error, msg);
                    filterContext.Result = new JsonResult() { Data = res };
                }
                else
                {
                    //filterContext.Result = new ContentResult() { Content = msg };
                    filterContext.Result = new RedirectResult("/Home/NoPermission");
                }
            }

        }
    }
}