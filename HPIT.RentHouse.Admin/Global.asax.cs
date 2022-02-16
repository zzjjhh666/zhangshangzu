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

            // ������־��¼
            FileInfo fileinfo = new FileInfo(Server.MapPath("~/1og4net.Config"));
            log4net.Config.XmlConfigurator.Configure(fileinfo);

            #region ioc����
            //1������ioc����������ʵ��
            var builder = new ContainerBuilder();
            //2���ѵ�ǰ�����е����е�Controller ��ע��
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //3����ȡ����������ĳ���
            Assembly[] assemblies = new Assembly[] { Assembly.Load("HPIT.RentHouse.Service") };
            //4����������������µĽӿ�����ķ���ʱ�򡣾ͻ᷵�ض�Ӧ��Services�������ʵ��
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract
                    && typeof(IServiceSupport).IsAssignableFrom(type))
                    .AsImplementedInterfaces().PropertiesAutowired();

            var container = builder.Build();
            //5��ע��ϵͳ�����DependencyResolver��������MVC��ܴ���Controller�ȶ����ʱ���ǹ�AutofacҪ����
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            GetUserInfo();
        }
        //ͨ��cookie���� ��ȡ�û���Ϣ�� HttpContext.Current.User
        public void GetUserInfo()
        {
            // 1. ����¼Cookie
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                try
                {
                    LoginAdminDTO userData = null;
                    // 2. ����Cookieֵ����ȡFormsAuthenticationTicket����
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                        // 3. ��ԭ�û�����
                        userData = JsonConvert.DeserializeObject<LoginAdminDTO>(ticket.UserData);

                    if (ticket != null && userData != null)
                        // 4. �������ǵ�MyFormsPrincipalʵ�������¸�context.User��ֵ��
                        HttpContext.Current.User = new MyFormsPrincipal<LoginAdminDTO>(ticket, userData);
                }
                catch { /* ���쳣Ҳ��Ҫ�׳�����ֹ��������̽�� */ }
            }
        }
    }
}
