using System.Web;
using System.Web.Mvc;

namespace _1291163_MVC_EF_Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
