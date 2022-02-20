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
    public class ContactController : Controller
    {
        ContactDAO contactDAO = new ContactDAO();      
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(contactDAO.getList("Index"));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
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
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM                              
                contact.Created_At = DateTime.Now;               
                TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                return RedirectToAction("Index", "Contact");
            }          
            return View(contact);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListCat = new SelectList(contactDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(contactDAO.getList("Index"), "Contact", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                contact.Updated_At = DateTime.Now;                
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }           
            return View(contact);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = contactDAO.getRow(id);          
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Contact");
        }
        public ActionResult Trash()
        {
            return View(contactDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Contact");
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Contact");
            }
            contact.Status = (contact.Status == 1) ? 2 : 1;
            contact.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            contact.Updated_At = DateTime.Now;
            contactDAO.Update(contact);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Contact");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Contact");
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Contact");
            }
            contact.Status = 0; // Trạng thái trác = 0
            contact.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            contact.Updated_At = DateTime.Now;
            contactDAO.Update(contact);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Contact");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Contact");
            }
            Contact contact = contactDAO.getRow(id);
            if (contact == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Contact");
            }
            contact.Status = 2; // Trạng thái trác = 0
            contact.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            contact.Updated_At = DateTime.Now;
            contactDAO.Update(contact);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Contact");
        }
    }
}
