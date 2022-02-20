using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ConfigDAO
    {
        private MyDBContext db = new MyDBContext();
        public int Insert(Config row)
        {
            db.Configs.Add(row);
            return db.SaveChanges();
        }
        // Cập nhật mẫu tin
        public int Update(Config row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Config row)
        {
            db.Configs.Remove(row);
            return db.SaveChanges();
        }
    }
}
