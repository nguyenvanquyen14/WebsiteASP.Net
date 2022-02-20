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
using System.IO;

namespace Shop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductDAO productDAO = new ProductDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productDAO.getListJoin("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);          
            return View("Create");
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM
                product.Slug = XString.Str_Slug(product.Name);
                product.Created_By = Convert.ToInt32(Session["UserId"].ToString());               
                product.Created_At = DateTime.Now;
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0) 
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if(FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = product.Slug + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/products/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        product.Img = imgName;
                    }    
                } 
                    
                    productDAO.Insert(product);
                TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                return RedirectToAction("Index", "Product");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = productDAO.getRow(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = XString.Str_Slug(product.Name);           
                product.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                product.Updated_At = DateTime.Now;
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0)
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if (FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = product.Slug + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/products/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        // Xóa hình cũ
                        if (product.Img != null)
                        {
                            string pathImgDelete = Path.Combine(Server.MapPath(pathDir), product.Img);
                            System.IO.File.Delete(pathImgDelete);
                        }
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        product.Img = imgName;
                    }
                }
                productDAO.Update(product);
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDAO.getList("Index"), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = productDAO.getRow(id);
            productDAO.Delete(product);
            
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Product");
        }
        public ActionResult Trash()
        {
            return View(productDAO.getListJoin("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Product");
            }
            product.Status = (product.Status == 1) ? 2 : 1;
            product.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            product.Updated_At = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Product");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Product");
            }
            product.Status = 0; // Trạng thái trác = 0
            product.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            product.Updated_At = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Product");
            }
            Product product = productDAO.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Product");
            }
            product.Status = 2; // Trạng thái trác = 0
            product.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            product.Updated_At = DateTime.Now;
            productDAO.Update(product);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Product");
        }
        //
        public string ProductImg(int? productid)
        {
            Product product = productDAO.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Img;
            }
        }
        public string ProductName(int? productid)
        {
            Product product = productDAO.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Name;
            }
        }
    }
}
