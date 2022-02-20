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
using System.IO;

namespace Shop.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SliderDAO sliderDAO = new SliderDAO();
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(sliderDAO.getList("Index"));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(sliderDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM
                slider.Link = XString.Str_Slug(slider.Name);
                slider.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                slider.Updated_At = DateTime.Now;
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0)
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if (FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = slider.Link + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/sliders/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        slider.Img = imgName;
                    }
                }
                sliderDAO.Insert(slider);            
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", "Thêm thành công");
            ViewBag.ListOrder = new SelectList(sliderDAO.getList("Index"), "Orders", "Name", 0);
            return View(slider);

        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListOrder = new SelectList(sliderDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                slider.Link = XString.Str_Slug(slider.Name);
                slider.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
                slider.Updated_At = DateTime.Now;
                //Upload file
                var fileImg = Request.Files["Img"];
                if (fileImg.ContentLength != 0)
                {
                    String[] FilExtensions = new string[] { ".jpg", ".png", ".gif", ".jepg" };
                    if (FilExtensions.Contains(fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."))))
                    {
                        string imgName = slider.Link + fileImg.FileName.Substring(fileImg.FileName.LastIndexOf("."));
                        string pathDir = "~/Public/images/sliders/";
                        string pathImg = Path.Combine(Server.MapPath(pathDir), imgName);
                        // Xóa hình cũ
                        if (slider.Img != null)
                        {
                            string pathImgDelete = Path.Combine(Server.MapPath(pathDir), slider.Img);
                            System.IO.File.Delete(pathImgDelete);
                        }
                        //Upload load file
                        fileImg.SaveAs(pathImg);
                        // Luu Hinh
                        slider.Img = imgName;
                    }
                }
                sliderDAO.Update(slider);
                ViewBag.ListOrder = new SelectList(sliderDAO.getList("Index"), "Orders", "Name", 0);
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", " Cập nhật thành công ");
            return View(slider);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = sliderDAO.getRow(id);
            sliderDAO.Delete(slider);
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Slider");
        }
        public ActionResult Trash()
        {
            return View(sliderDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Slider");
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Slider");
            }
            slider.Status = (slider.Status == 1) ? 2 : 1;
            slider.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            slider.Updated_At = DateTime.Now;
            sliderDAO.Update(slider);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Slider");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Slider");
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Slider");
            }
            slider.Status = 0; // Trạng thái trác = 0
            slider.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            slider.Updated_At = DateTime.Now;
            sliderDAO.Update(slider);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Slider");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Slider");
            }
            Slider slider = sliderDAO.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Slider");
            }
            slider.Status = 2; // Trạng thái trác = 0
            slider.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            slider.Updated_At = DateTime.Now;
            sliderDAO.Update(slider);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Slider");
        }
    }
}
