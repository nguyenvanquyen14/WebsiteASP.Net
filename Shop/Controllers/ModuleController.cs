using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace Shop.Controllers
{
    public class ModuleController : Controller
    {
        private MenuDAO menuDAO = new MenuDAO();
        private SliderDAO silderDAO = new SliderDAO();
        private CategoryDAO categoryDAO = new CategoryDAO();
        // GET: Module
        public ActionResult MainMenu()
        {
            List<Menu> list = menuDAO.getlistByParentId("mainmenu",0);
            return View("MainMenu", list);
        }
        public ActionResult MainMenuSub(int id)
        {
            Menu menu = menuDAO.getRow(id);
            List<Menu> list = menuDAO.getlistByParentId("mainmenu", id);
            if(list.Count==0)
            {
                return View("MainMenuSub1", menu); // không có cấp con
            }   
            else
            {
                ViewBag.Menu = menu;
                return View("MainMenuSub2", list); // Có cấp con
            }                
        }
        // Slideshow
        public ActionResult Slideshow()
        {
            List<Slider> list = silderDAO.getListByPosition("Slideshow");
            return View("Slideshow", list);
        }
        //ListCategory
        public ActionResult ListCategory()
        {
            List<Category> list = categoryDAO.getListByParentId(0);
            return View("ListCategory", list);
        }
        public ActionResult MenuFooter()
        {
            List<Menu> list = menuDAO.getlistByParentId("footermenu", 0);
            return View("MenuFooter", list);
        }
    }
}