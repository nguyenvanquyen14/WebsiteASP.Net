using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class MenuDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Menu> getlistByParentId( string position, int parentid=0)
        {
            return db.Menus
                .Where(m=>m.Parentid== parentid && m.Status==1 && m.Position==position)
                .OrderBy(m=>m.Orders)
                .ToList(); // chưa có điều kiện
        }
        public List<Menu> getList(string status = "All")
        {
            List<Menu> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Menus.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Menus.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Menus.ToList();
                        break;
                    }
            }
            return list;
        }
        public Menu getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Menus.Find(id);
            }
        }
        public int Insert(Menu row)
        {
            db.Menus.Add(row);
            return db.SaveChanges();
        }
        // Cập nhật mẫu tin
        public int Update(Menu row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Menu row)
        {
            db.Menus.Remove(row);
            return db.SaveChanges();
        }
    }
}
