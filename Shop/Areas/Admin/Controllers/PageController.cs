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
    public class PageController : Controller
    {
        private PostDAO postDAO = new PostDAO();
        private LinkDAO linkDAO = new LinkDAO();

        // GET: Admin/Page
        public ActionResult Index()
        {
            return View(postDAO.getList("Index", "Page"));
        }

        // GET: Admin/Page/Details/5
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

        // GET: Admin/Page/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Page/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.Type = "Page";
                post.Slug = XString.Str_Slug(post.Title);
                post.Created_At = DateTime.Now;
                post.Created_By = (Session["UserId"].Equals("")) ? 1 : int.Parse(Session["UserId"].ToString());
                if(postDAO.Insert(post)==1)
                {
                    Link link = new Link();
                    link.Slug = post.Slug;
                    link.TableId = post.Id;
                    link.TypeLink = "page";
                    linkDAO.Insert(link);
                    TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                }
                
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Admin/Page/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Admin/Page/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.Type = "Page";
                post.Slug = XString.Str_Slug(post.Title);
                post.Updated_At = DateTime.Now;
                post.Updated_By = (Session["UserId"].Equals("")) ? 1 : int.Parse(Session["UserId"].ToString());
                if (postDAO.Update(post) == 1)
                {
                    Link link = linkDAO.getRow(post.Id, "page");
                    link.Slug = post.Slug;
                    linkDAO.Update(link);
                    TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                }
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Page/Delete/5
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

        // POST: Admin/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = postDAO.getRow(id);
            return RedirectToAction("Index");
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
                return RedirectToAction("Index", "Page");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Page");
            }
            post.Status = (post.Status == 1) ? 2 : 1;
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Page");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Page");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Page");
            }
            post.Status = 0; // Trạng thái trác = 0
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Page");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Page");
            }
            Post post = postDAO.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Page");
            }
            post.Status = 2; // Trạng thái trác = 0
            post.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            post.Updated_At = DateTime.Now;
            postDAO.Update(post);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Page");
        }
    }
}
