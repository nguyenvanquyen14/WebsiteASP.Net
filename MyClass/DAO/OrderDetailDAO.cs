using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class OrderDetailDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Orderdetail> getList(int? orderid)
        {
            return db.Orderdetails.Where(m=>m.Orderid== orderid).ToList();
        }
        public int Insert(Orderdetail row)
        {
            db.Orderdetails.Add(row);
            return db.SaveChanges();
        }
        // Cập nhật mẫu tin
        public int Update(Orderdetail row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Orderdetail row)
        {
            db.Orderdetails.Remove(row);
            return db.SaveChanges();
        }
    }
}
