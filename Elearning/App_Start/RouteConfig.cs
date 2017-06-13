using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Elearning
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Course Category",
                url: "khoa-hoc/{metatitle}-{cateId}",
                defaults: new { controller = "Course", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "Elearning.Controllers" } //chỉ ra home index cho trang client để phân biệt với trang admin
            );

            routes.MapRoute(
                name: "Course Detail",
                url: "chi-tiet/{metatitle}-{detaId}",
                defaults: new { controller = "Course", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "Elearning.Controllers" } //chỉ ra home index cho trang client để phân biệt với trang admin
            );

            routes.MapRoute(
               name: "Lecture Detail",
               url: "Course/LectureContent/{detalecId}",
               defaults: new { controller = "Course", action = "LectureContent", id = UrlParameter.Optional },
               namespaces: new[] { "Elearning.Controllers" } //chỉ ra home index cho trang client để phân biệt với trang admin
           );

            routes.MapRoute(
             name: "Cart",
             url: "gio-hang",
             defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "Elearning.Controllers" }
          );

            routes.MapRoute(
              name: "Add Cart",
              url: "them-gio-hang",
              defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
              namespaces: new[] { "Elearning.Controllers" }
           );

            routes.MapRoute(
             name: "Payment",
             url: "thanh-toan",
             defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
             namespaces: new[] { "Elearning.Controllers" }
          );

            routes.MapRoute(
            name: "Payment Success",
            url: "hoan-thanh",
            defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
            namespaces: new[] { "Elearning.Controllers" }
         );

            routes.MapRoute(
              name: "Login",
              url: "dang-nhap",
              defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
              namespaces: new[] { "Elearning.Controllers" }
           );

            routes.MapRoute(
               name: "Register",
               url: "dang-ky",
               defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
               namespaces: new[] { "Elearning.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Elearning.Controllers" } //chỉ ra home index cho trang client để phân biệt với trang admin
            );

        }

        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //}
    }
}
