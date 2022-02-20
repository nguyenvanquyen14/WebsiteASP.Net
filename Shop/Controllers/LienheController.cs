using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace Shop.Controllers
{
    public class LienheController : Controller
    {
        private MyDBContext db = new MyDBContext();
        ContactDAO contactDAO = new ContactDAO();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Contact contact)
        {                   
                if (ModelState.IsValid)
            {
                contact.Status = 1;
                contact.Created_At = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                ViewBag.Error = "Nguyên Văn Quyên";
                return RedirectToAction("Index");
            }

            return View(contact);
        }
    }
}