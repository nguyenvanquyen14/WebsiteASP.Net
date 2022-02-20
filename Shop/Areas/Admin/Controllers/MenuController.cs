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
    public class MenuController : Controller
    {
        CategoryDAO categoryDAO = new CategoryDAO();
        TopicDAO topicDao = new TopicDAO();
        PostDAO postDAO = new PostDAO();
        MenuDAO menuDAO = new MenuDAO();
        SupplierDAO supplierDAO = new SupplierDAO();
        
        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.ListCategory = categoryDAO.getList("Index");
            ViewBag.ListTopic = topicDao.getList("Index");
            ViewBag.ListPage = postDAO.getList("Index", "Page");
            List<Menu> menu = menuDAO.getList("Index");
            return View("Index", menu);
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                if (!string.IsNullOrEmpty(form["itemCategory"]))
                {
                    var listitem = form["itemCategory"];
                    var listarr = listitem.Split(',');
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);
                        Category category = categoryDAO.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = category.Name;
                        menu.Link = category.Slug;
                        menu.Tableid = category.Id;
                        menu.Type = "category";
                        menu.Position = form["Position"];
                        menu.Parentid = 0;
                        menu.Orders = 0;
                        menu.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                        menu.Created_At = DateTime.Now;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }

            //Topic
            if (!string.IsNullOrEmpty(form["ThemTopic"]))
            {
                if (!string.IsNullOrEmpty(form["nametopic"]))
                {
                    var listitem = form["nametopic"];
                    var listarr = listitem.Split(',');
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);
                        Topic topic = topicDao.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = topic.Name;
                        menu.Link = topic.Slug;
                        menu.Tableid = topic.Id;
                        menu.Type = "content";
                        menu.Position = form["Position"];
                        menu.Parentid = 0;
                        menu.Orders = 0;
                        menu.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                        menu.Created_At = DateTime.Now;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }
            //Page
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["namepage"]))
                {
                    var listitem = form["namepage"];
                    var listarr = listitem.Split(',');
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);
                        Post post = postDAO.getRow(id);
                        Menu menu = new Menu();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.Tableid = post.Id;
                        menu.Type = "page";
                        menu.Position = form["Position"];
                        menu.Parentid = 0;
                        menu.Orders = 0;
                        menu.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                        menu.Created_At = DateTime.Now;
                        menu.Status = 2;
                        menuDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }
            // ThêmCustom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                if (!string.IsNullOrEmpty(form["name"]) && !string.IsNullOrEmpty(form["link"]))
                {
                    Menu menu = new Menu();
                    menu.Name = form["name"];
                    menu.Link = form["link"];
                    menu.Type = "custom";
                    menu.Position = form["Position"];
                    menu.Parentid = 0;
                    menu.Orders = 0;
                    menu.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                    menu.Created_At = DateTime.Now;
                    menu.Status = 2;
                    menuDAO.Insert(menu);
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa nhập đủ thông tin");
                }
            }
            return RedirectToAction("Index", "Menu");
        }
        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuDAO.getRow(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuDAO.getRow(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = menuDAO.getRow(id);
            menuDAO.Delete(menu);
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Menu");
        }
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListMenu = new SelectList(menuDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(menuDAO.getList("Index"), "Orders", "Name", 0);
            Menu menu = menuDAO.getRow(id);
            return View("Edit",menu);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.Parentid == null)
                {
                    menu.Parentid = 0;
                }
                if (menu.Orders == null)
                {
                    menu.Orders = 1;
                }
                else
                {
                    menu.Orders += 1;
                }
                menu.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                menu.Updated_At = DateTime.Now;
                menuDAO.Update(menu);
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }
            ViewBag.ListMenu = new SelectList(menuDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(menuDAO.getList("Index"), "Orders", "Name", 0);
            return View(menu);
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index");
            }
            Menu menu = menuDAO.getRow(id);
            if (menu == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index");
            }
            menu.Status = (menu.Status == 1) ? 2 : 1;
            menu.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            menu.Updated_At = DateTime.Now;
            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            return View(menuDAO.getList("Trash"));
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index");
            }
            Menu menu = menuDAO.getRow(id);
            if (menu == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index");
            }
            menu.Status = 0; // Trạng thái trác = 0
            menu.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            menu.Updated_At = DateTime.Now;
            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash");
            }
            Menu menu = menuDAO.getRow(id);
            if (menu == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash");
            }
            menu.Status = 2; // Trạng thái trác = 0
            menu.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            menu.Updated_At = DateTime.Now;
            menuDAO.Update(menu);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash");
        }
    }
}
