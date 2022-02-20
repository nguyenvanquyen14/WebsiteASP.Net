 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            //Lưu mã người đăng nhập
            Session["UserId"] = 1;
            //Lưu thông tin người quản lý
            Session["UserAdmin"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            // Giỏ hàng
            Session["MyCart"] = "";
            // Lưu thông tin đăng nhập người dùng
            Session["UserCustomer"] = "";
            Session["CustomerId"] = "";
        }
    }
}
