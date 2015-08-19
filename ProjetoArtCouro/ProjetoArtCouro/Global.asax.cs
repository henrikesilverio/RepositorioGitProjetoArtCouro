using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ProjetoArtCouro.Web.Infra.AutoMapper;

namespace ProjetoArtCouro.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Registrando o AutoMapper das entidades criadas no projeto
            AutoMapperConfig.RegisterMappings();
        }
    }
}