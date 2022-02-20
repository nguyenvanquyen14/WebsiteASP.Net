using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;
using Shop;

namespace Shop.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SupplierDAO supplierDAO = new SupplierDAO();
        LinkDAO linkDAO = new LinkDAO();
        // GET: Admin/supplier
        public ActionResult Index()
        {
            return View(supplierDAO.getList("Index"));
        }

        // GET: Admin/supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/supplier/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM
                supplier.Slug = XString.Str_Slug(supplier.Name);
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders += 1;
                }
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0)
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if (FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = supplier.Slug + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/suppliers/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        supplier.Img = imgName;
                    }
                }
                supplier.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                supplier.Created_At = DateTime.Now;
                if (supplierDAO.Insert(supplier) == 1)
                {
                    Link link = new Link();
                    link.Slug = supplier.Slug;
                    link.TableId = supplier.Id;
                    link.TypeLink = "supplier";
                    linkDAO.Insert(link);
                }
                TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                return RedirectToAction("Index", "supplier");
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // GET: Admin/supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // POST: Admin/supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.Slug = XString.Str_Slug(supplier.Name);
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders += 1;
                }
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0)
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if (FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = supplier.Slug + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/suppliers/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        // Xóa hình cũ
                        if (supplier.Img != null)
                        {
                            string pathImgDelete = Path.Combine(Server.MapPath(pathDir), supplier.Img);
                            System.IO.File.Delete(pathImgDelete);
                        }
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        supplier.Img = imgName;
                    }
                }
                supplier.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                supplier.Updated_At = DateTime.Now;
                if (supplierDAO.Update(supplier) == 1)
                {
                    Link link = linkDAO.getRow(supplier.Id, "supplier");
                    link.Slug = supplier.Slug;
                    linkDAO.Update(link);

                }
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(supplierDAO.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // GET: Admin/supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = supplierDAO.getRow(id);
            Link link = linkDAO.getRow(supplier.Id, "supplier");
            string PathDir = "~/Public/images/suppliers/";
            // Xóa hình ảnh 
            if (supplier.Img != null)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Img);
                System.IO.File.Delete(DelPath);// xáo hình
            } 
            if (supplierDAO.Delete(supplier) == 1)
            {
                linkDAO.Delete(link);
            }
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "supplier");
        }
        public ActionResult Trash()
        {
            return View(supplierDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "supplier");
            }
            supplier.Status = (supplier.Status == 1) ? 2 : 1;
            supplier.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            supplier.Updated_At = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "supplier");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "supplier");
            }
            supplier.Status = 0; // Trạng thái trác = 0
            supplier.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            supplier.Updated_At = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "supplier");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "supplier");
            }
            Supplier supplier = supplierDAO.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "supplier");
            }
            supplier.Status = 2; // Trạng thái trác = 0
            supplier.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            supplier.Updated_At = DateTime.Now;
            supplierDAO.Update(supplier);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "supplier");
        }
    }
}
