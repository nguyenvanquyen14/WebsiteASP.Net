using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SliderDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Slider> getListByPosition(string position)
        {
            return db.Sliders.Where(m=>m.Position==position && m.Status==1).OrderBy(m=>m.Orders).ToList();
        }
        public List<Slider> getList(string status = "All")
        {
            List<Slider> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Sliders.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Sliders.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Sliders.ToList();
                        break;
                    }
            }
            return list;
        }
        public Slider getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Sliders.Find(id);
            }
        }
        public int Insert(Slider row)
        {
            db.Sliders.Add(row);
            return db.SaveChanges();
        }
        // Cập nhật mẫu tin
        public int Update(Slider row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Slider row)
        {
            db.Sliders.Remove(row);
            return db.SaveChanges();
        }
    }
}
