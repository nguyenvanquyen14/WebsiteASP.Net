using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;

namespace Shop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        UserDAO userDAO = new UserDAO();             
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(userDAO.getList("Index"));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {          
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM
                user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                user.Updated_At = DateTime.Now;
                userDAO.Insert(user);
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", "Thêm thành công");
            return View(user);
            
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {

                user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                user.Updated_At = DateTime.Now;
                userDAO.Update(user);              
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", " Cập nhật thành công ");
            return View(user);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = userDAO.getRow(id);
            userDAO.Delete(user);
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "User");
        }
        public ActionResult Trash()
        {
            return View(userDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "User");
            }
            user.Status = (user.Status == 1) ? 2 : 1;
            user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            user.Updated_At = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "User");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "User");
            }
            user.Status = 0; // Trạng thái trác = 0
            user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            user.Updated_At = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "User");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "User");
            }
            User user = userDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "User");
            }
            user.Status = 2; // Trạng thái trác = 0
            user.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            user.Updated_At = DateTime.Now;
            userDAO.Update(user);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "User");
        }
        public string NameCustomer(int userid)
        {
            User user = userDAO.getRow(userid);
            if(user==null)
            {
                return "";
            } 
            else
            {
                return user.Fullname;
            }    
        }
    }
}
