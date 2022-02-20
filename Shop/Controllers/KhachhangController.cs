using Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace Shop.Controllers
{
    public class KhachhangController : Controller
    {
        MyDBContext objModel = new MyDBContext();
        UserDAO userDAO = new UserDAO();
        // GET: Khachhang
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection filed)
        {
            string username = filed["username"];
            string password = XString.ToMD5(filed["password"]);
            //
            User row_user = userDAO.getRow(username, "Customer");
            String strError = "";
            if (row_user == null)
            {
                strError = "Tên đăng nhập không tồn tại";
            }
            else
            {
                if (password.Equals(row_user.Password))
                {
                    Session["UserCustomer"] = username;
                    Session["CustomerId"] = row_user.Id;
                    return Redirect("~/");
                }
                else
                {
                    strError = password;
                }
            }
            ViewBag.Error = "<span class='text-danger'>" + strError + "</div>";
            return View("DangNhap");
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(User user)
        {
            var check = objModel.Users.FirstOrDefault(s => s.Username == user.Username);
            if (check == null)
            {
                // XỬ LÝ THÊM
                user.Roles = "Customer";
                user.Status = 1;
                user.Password = XString.ToMD5(user.Password);
                user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                user.Updated_At = DateTime.Now;
                objModel.Configuration.ValidateOnSaveEnabled = false;
                objModel.Users.Add(user);
                objModel.SaveChanges();
            }
            else
            {
                ViewBag.error = "Tài khoản đã tồn tại";
                return View();
            }
            ViewBag.Error = "<span class='text-danger'>Đăng kí thành công</ div > ";
            return View("DangNhap");
        }
        public ActionResult DangXuat()
        {
            Session["UserCustomer"] = "";
            Session["CustomerId"] = "";
            return RedirectToAction("DangNhap");
        }
    }
}