using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class PostDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Post> getListByTopicId(int? topid, string type = "Post", int? notid = null)
        {
            List<Post> list = null;
            if (notid == null)
            {
                list = db.Posts.Where(m => m.Status == 1 && m.Type == type && m.Topid == topid).ToList();
            }
            else
            {
                list = db.Posts.Where(m => m.Status == 1 && m.Type == type && m.Topid == topid && m.Id != notid).ToList();
            }
            return list;
        }
        public List<Post> getList(string status = "All", string type = "Post")
        {
            List<Post> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts.Where(m => m.Status != 0 && m.Type == type).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts.Where(m => m.Status == 0 && m.Type == type).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts.Where(m=>m.Type==type).ToList();
                        break;
                    }
            }
            return list;
        }
        public Post getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);
            }
        }
        public Post getRow(string slug)
        {
            return db.Posts.Where(m => m.Type == "post" && m.Slug == slug && m.Status == 1).FirstOrDefault();
        }
        public Post getRow(string slug, string posttype)
        {
            return db.Posts
                .Where(m =>m.Slug == slug && m.Type == posttype && m.Status == 1).FirstOrDefault();
        }
        public int Insert(Post row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }

        // Cập nhật mẫu tin
        public int Update(Post row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Post row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}
