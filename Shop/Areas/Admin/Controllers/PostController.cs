using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;
using Shop;

namespace Shop.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        PostDAO postDAO = new PostDAO();
        TopicDAO topicDAO = new TopicDAO();
        // GET: Admin/Post
        public ActionResult Index()
        {
            return View(postDAO.getList("Index","Post"));
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Post/Create
        public ActionResult Create()
        {
            ViewBag.ListTop = new SelectList(topicDAO.getList("Index"), "Id", "Name");
            return View();
        }

        // POST: Admin/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM
                post.Slug = XString.Str_Slug(post.Title);
                post.Type = "Post";         
                post.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                post.Created_At = DateTime.Now;
                postDAO.Insert(post);
                
                TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                return RedirectToAction("Index", "Post");
            }
            ViewBag.ListTop = new SelectList(topicDAO.getList("Index"), "Id", "Name");
            return View(post);
        }

        // GET: Admin/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListTop = new SelectList(topicDAO.getList("Index"), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.Slug = XString.Str_Slug(post.Title);
                post.Type = "Post";            
                post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                post.Updated_At = DateTime.Now;
                postDAO.Update(post);
               
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }
            ViewBag.ListTop = new SelectList(topicDAO.getList("Index"), "Id", "Name");
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post Post = postDAO.getRow(id);         
            postDAO.Delete(Post);
           
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Post");
        }
        public ActionResult Trash()
        {
            return View(postDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Post");
            }
            post.Status = (post.Status == 1) ? 2 : 1;
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Post");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Post");
            }
            post.Status = 0; // Trạng thái trác = 0
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Post");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Post");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Post");
            }
            post.Status = 2; // Trạng thái trác = 0
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Post");
        }
    }
}
