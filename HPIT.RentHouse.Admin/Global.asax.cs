using Autofac;
using Autofac.Integration.Mvc;
using HPIT.RentHouse.DTO;
using HPIT.RentHouse.lService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace HPIT.RentHouse.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 启用日志记录
            FileInfo fileinfo = new FileInfo(Server.MapPath("~/1og4net.Config"));
            log4net.Config.XmlConfigurator.Configure(fileinfo);

            #region ioc配置
            //1、创建ioc容器，创建实例
            var builder = new ContainerBuilder();
            //2、把当前程序集中的所有的Controller 都注册
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //3、获取所有相关类库的程序集
            Assembly[] assemblies = new Assembly[] { Assembly.Load("HPIT.RentHouse.Service") };
            //4、当请求这个程序集下的接口里面的方法时候。就会返回对应的Services类里面的实现
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract
                    && typeof(IServiceSupport).IsAssignableFrom(type))
                    .AsImplementedInterfaces().PropertiesAutowired();

            var container = builder.Build();
            //5、注册系统级别的DependencyResolver，这样当MVC框架创建Controller等对象的时候都是管Autofac要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            GetUserInfo();
        }
        //通过cookie解密 读取用户信息到 HttpContext.Current.User
        public void GetUserInfo()
        {
            // 1. 读登录Cookie
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                try
                {
                    LoginAdminDTO userData = null;
                    // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                        // 3. 还原用户数据
                        userData = JsonConvert.DeserializeObject<LoginAdminDTO>(ticket.UserData);

                    if (ticket != null && userData != null)
                        // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。
                        HttpContext.Current.User = new MyFormsPrincipal<LoginAdminDTO>(ticket, userData);
                }
                catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
            }
        }
    }
}
