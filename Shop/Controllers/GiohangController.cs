using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace Shop.Controllers
{
    public class GiohangController : Controller
    {
        ProductDAO productDAO = new ProductDAO();
        UserDAO userDAO = new UserDAO();
        OrderDAO orderDAO = new OrderDAO();
        OrderDetailDAO orderdetailDAO = new OrderDetailDAO();
        XCart xcart = new XCart();
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> listcart = xcart.getCart();
            return View("Index", listcart);
        }
        public ActionResult CartAdd( int productid)
        {
            Product product = productDAO.getRow(productid);
            CartItem cartitem = new CartItem(product.Id, product.Name, product.Img, product.PriceSale, 1);
            // Add vào giỏ hàng
            List<CartItem> listcart = xcart.AddCart(cartitem);          
            return RedirectToAction("Index", "Giohang");
        }
        public ActionResult CartDel(int productid)
        {
            xcart.DelCart(productid);
            return RedirectToAction("Index", "Giohang");
        }
        //CartUpdate
        public ActionResult CartUpdate(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["CAPNHAT"]))
            {
                var listqty = form["qty"];
                var listarr = listqty.Split(',');
                xcart.UpdateCart(listarr);                
            }
            return RedirectToAction("Index", "Giohang");
        }
        //CartDelAll
        public ActionResult CartDelAll()
        {
            xcart.DelCart();
            return RedirectToAction("Index", "Giohang");
        }
        //ThanhToan
        public ActionResult ThanhToan()
        {
            
            List<CartItem> listcart = xcart.getCart();
            // kiểm tra đăng nhập trang người dùng ==> Khách hàng
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap");// chuyeent hướng trang đến URL
            }
            int userid = int.Parse(Session["CustomerId"].ToString()); // Mã khách hàng
            User user = userDAO.getRow(userid);
            ViewBag.user = user;
            return View("ThanhToan", listcart);
        }
        public ActionResult DatMua(FormCollection field)
        {
            // Lưu thông tin vào csdl Order và OrderDetail
            int userid = int.Parse(Session["CustomerId"].ToString()); // Mã khách hàng
            User user = userDAO.getRow(userid);
            // Lấy thông tin
            String note = field["Note"];
            // Tọa đối tượng
            Order order = new Order();
            order.Userid = userid;
            order.Note = note;
            order.Status = 1;
            order.Created_At = DateTime.Now;
            if(orderDAO.Insert(order) == 1)
            {
                // thêm vào  chi tiết đơn hàng
                List<CartItem> listcart = xcart.getCart();
                foreach (CartItem cartItem in listcart)
                {
                    Orderdetail orderdetail = new Orderdetail();
                    orderdetail.Orderid = order.Id;
                    orderdetail.Productid = cartItem.ProductId;
                    orderdetail.Price = cartItem.PriceSale;
                    orderdetail.Qty = cartItem.Qty;
                    orderdetail.Amount = cartItem.Amount;
                    orderdetailDAO.Insert(orderdetail); // Lưu
                }
            }
            return Redirect("~/thanh-cong");// chuyển hướng trang đến URL
        }
        public ActionResult ThanhCong()
        {
            List<CartItem> listcart = xcart.getCart();
            return View("ThanhCong", listcart);
        }
    }
}