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
    public class OrderController : Controller
    {
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO orderDetailDAO = new OrderDetailDAO();
        // GET: Admin/Order
        public ActionResult Index()
        {
            List<Order> list = orderDAO.getList("Index");
            return View(list);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListChiTiet = orderDetailDAO.getList(id);
            return View(order);
        }

        // GET: Admin/Order/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(orderDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(orderDAO.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                // XỬ LÝ THÊM             
                order.Created_By = Convert.ToInt32(Session["UserId"].ToString());
                order.Created_At = DateTime.Now;
                TempData["message"] = new XMessage("success", " Thêm mẫu tin thành công ");
                return RedirectToAction("Index", "Order");
            }
            ViewBag.ListCat = new SelectList(orderDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(orderDAO.getList("Index"), "Orders", "Name", 0);
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListCat = new SelectList(orderDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(orderDAO.getList("Index"), "Orders", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = new XMessage("success", " Cập nhật thành công ");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(orderDAO.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(orderDAO.getList("Index"), "Orders", "Name", 0);
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = orderDAO.getRow(id);
            orderDAO.Delete(order);
            TempData["message"] = new XMessage("success", " Xóa mẫu tin thành công ");
            return RedirectToAction("Trash", "Order");
        }
        public ActionResult Trash()
        {
            return View(orderDAO.getList("Trash"));
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Order");
            }
            order.Status = (order.Status == 1) ? 2 : 1;
            order.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            order.Updated_At = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Thay đổi trạng thái thành công ");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Order");
            }
            order.Status = 0; // Trạng thái trác = 0
            order.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            order.Updated_At = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Xóa vào thùng rác thành công ");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", " Mã loại sản phẩm không tồn tại ");
                return RedirectToAction("trash", "Order");
            }
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("trash", "Order");
            }
            order.Status = 2; // Trạng thái trác = 0
            order.Updated_By = Convert.ToInt32(Session["UserId"].ToString());
            order.Updated_At = DateTime.Now;
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Khôi phục thành công ");
            return RedirectToAction("trash", "Order");
        }
        public ActionResult Destroy(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index", "Order");
            }
            if (order.Status == 1 || order.Status == 2)
            {
                order.Status = 0;
                order.Updated_At = DateTime.Now;
                order.Updated_By = 1;
            }
            else
            {
                if (order.Status == 3)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng đã vân chuyển không thể hủy ");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng đã giao không thể hủy ");
                }

                return RedirectToAction("Index", "Order");
            }
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Đã hủy đơn hàng thành công ");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult DaXacMinh(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index");
            }
            if (order.Status == 1)
            {
                order.Status = 2;
                order.Updated_At = DateTime.Now;
                order.Updated_By = 1;
            }
            else
            {
                if (order.Status == 3)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng đã vân chuyển không thể hủy ");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng đã giao không thể hủy ");
                }

                return RedirectToAction("Index");
            }
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Đã xác minh đơn hàng thành công ");
            return RedirectToAction("Index");
        }
        public ActionResult DangVanChuyen(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index");
            }
            if (order.Status == 2)
            {
                order.Status = 3;
                order.Updated_At = DateTime.Now;
                order.Updated_By = 1;
            }
            else
            {
                if (order.Status == 1)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng chưa được xác minh  ");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng đang vận chuyển không thể hủy ");
                }

                return RedirectToAction("Index");
            }
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", " Đang vận chuyển đơn hàng thành công ");
            return RedirectToAction("Index");
        }
        public ActionResult ThanhCong(int? id)
        {
            Order order = orderDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", " Mẫu tin không tồn tại ");
                return RedirectToAction("Index");
            }
            if (order.Status == 3)
            {
                order.Status = 4;
                order.Updated_At = DateTime.Now;
                order.Updated_By = 1;
            }
            else
            {
                if (order.Status == 1)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng chưa được xác minh  ");
                }
                if (order.Status == 2)
                {
                    TempData["message"] = new XMessage("danger", " Đơn hàng chưa được vận chuyển ");
                }

                return RedirectToAction("Index");
            }
            orderDAO.Update(order);
            TempData["message"] = new XMessage("success", "  Thành công ");
            return RedirectToAction("Index");
        }
    }
}
