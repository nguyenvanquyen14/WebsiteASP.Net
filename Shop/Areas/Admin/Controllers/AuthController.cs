
using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private MyDBContext db = new MyDBContext();
        // GET: Admin/Auth
        public ActionResult Login()
        {
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public ActionResult DoLogin( FormCollection filed)
        {
            ViewBag.Error = "";
            string username = filed["username"];
            string password = filed["password"];
            // Kiểm tra trong csdl có username 
            User user = db.Users.Where(m => m.Roles == "Admin" && m.Status == 1 && (m.Username == username || m.Email == username))
                 .FirstOrDefault();
            if(user!=null)
            {
                if(user.CountError >= 5 && user.Roles!="Admin")
                {
                    ViewBag.Error = " <p class='login-box-msg text-danger'> Liên hệ người quản lý !</p>";
                }    
                else
                {
                    if (user.Password.Equals(password))
                    {
                        Session["UserId"] = user.Id.ToString();
                        Session["UserAdmin"] = username;
                        Session["FullName"] = user.Fullname;
                        Session["Img"] = user.Img;
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (user.CountError == null)
                        {
                            user.CountError = 1;
                        }
                        else
                        {
                            user.CountError += 1;
                        }
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Error = " <p class='login-box-msg text-danger'> Mật khẩu không tồn tại !</p>";
                    }
                }   
            }    
            else
            {
                ViewBag.Error = " <p class='login-box-msg text-danger'> Tài khoản <strong>" + username+"</strong> không tồn tại !</p>";
            }    
            return View("Login");
        }
        public ActionResult Logout()
        {
            Session["UserId"] = "";
            Session["UserAdmin"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            return Redirect("~/Admin/Login");
        }
    }
}