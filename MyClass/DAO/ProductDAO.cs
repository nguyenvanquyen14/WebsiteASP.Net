using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace MyClass.DAO
{
    public class ProductDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<ProductInfo> getListByListCatId(List<int>listcatid, int take)
        {
            List<ProductInfo> list = db.Products
                 .Join(
                            db.Categorys,
                            p => p.Catid,
                            c => c.Id,
                            (p, c) => new ProductInfo
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Catid = p.Catid,
                                CategoryName = c.Name,
                                Slug = p.Slug,
                                Img = p.Img,
                                Detail = p.Detail,
                                Number = p.Number,
                                Price = p.Price,
                                PriceSale = p.PriceSale,
                                Metakey = p.Metakey,
                                MetaDesc = p.MetaDesc,
                                Created_By = p.Created_By,
                                Created_At = p.Created_At,
                                Updated_By = p.Updated_By,
                                Updated_At = p.Updated_At,
                                Status = p.Status,
                            }
                            )
                .Where(m=>m.Status==1 && listcatid.Contains(m.Catid))
                .OrderByDescending(m=>m.Created_At)
                .Take(take)
                .ToList();
            return list;
        }
        public IPagedList<ProductInfo> getListByListCatId(List<int> listcatid, int pageSize, int pageNumber)
        {
            IPagedList<ProductInfo> list = db.Products
                 .Join(
                            db.Categorys,
                            p => p.Catid,
                            c => c.Id,
                            (p, c) => new ProductInfo
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Catid = p.Catid,
                                CategoryName = c.Name,
                                Slug = p.Slug,
                                Img = p.Img,
                                Detail = p.Detail,
                                Number = p.Number,
                                Price = p.Price,
                                PriceSale = p.PriceSale,
                                Metakey = p.Metakey,
                                MetaDesc = p.MetaDesc,
                                Created_By = p.Created_By,
                                Created_At = p.Created_At,
                                Updated_By = p.Updated_By,
                                Updated_At = p.Updated_At,
                                Status = p.Status,
                            }
                            )
                .Where(m => m.Status == 1 && listcatid.Contains(m.Catid))
                .OrderByDescending(m => m.Created_At)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }
        public IPagedList<ProductInfo> getList(int pageSize, int pageNumber)
        {
            IPagedList<ProductInfo> list = db.Products
                .Join(
                           db.Categorys,
                           p => p.Catid,
                           c => c.Id,
                           (p, c) => new ProductInfo
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Catid = p.Catid,
                               CategoryName = c.Name,
                               Slug = p.Slug,
                               Img = p.Img,
                               Detail = p.Detail,
                               Number = p.Number,
                               Price = p.Price,
                               PriceSale = p.PriceSale,
                               Metakey = p.Metakey,
                               MetaDesc = p.MetaDesc,
                               Created_By = p.Created_By,
                               Created_At = p.Created_At,
                               Updated_By = p.Updated_By,
                               Updated_At = p.Updated_At,
                               Status = p.Status,
                           }
                           )
               .Where(m => m.Status == 1)
               .OrderByDescending(m => m.Created_At)
               .ToPagedList(pageNumber,pageSize);
            return list;
        }
        public List<ProductInfo> getListJoin(string status = "All")
        {
            List<ProductInfo> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products
                            .Join(
                            db.Categorys,
                            p=>p.Catid,
                            c=>c.Id,
                            (p,c) => new ProductInfo
                            {
                                 Id = p.Id,
                                 Name = p.Name,
                                Catid = p.Catid,
                                CategoryName = c.Name,
                                Slug = p.Slug,
                                Img = p.Img,
                                Detail = p.Detail,
                                Number = p.Number,
                                Price = p.Price,
                                PriceSale = p.PriceSale,
                                Metakey = p.Metakey,
                                MetaDesc = p.MetaDesc,
                                Created_By = p.Created_By,
                                Created_At = p.Created_At,
                                Updated_By = p.Updated_By,
                                Updated_At = p.Updated_At,
                                Status = p.Status,
                            }
                            ).Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products.Join(
                            db.Categorys,
                            p => p.Catid,
                            c => c.Id,
                            (p, c) => new ProductInfo
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Catid = p.Catid,
                                CategoryName = c.Name,
                                Slug = p.Slug,
                                Img = p.Img,
                                Detail = p.Detail,
                                Number = p.Number,
                                Price = p.Price,
                                PriceSale = p.PriceSale,
                                Metakey = p.Metakey,
                                MetaDesc = p.MetaDesc,
                                Created_By = p.Created_By,
                                Created_At = p.Created_At,
                                Updated_By = p.Updated_By,
                                Updated_At = p.Updated_At,
                                Status = p.Status,
                            }
                            ).Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products
                             .Join(
                            db.Categorys,
                            p => p.Catid,
                            c => c.Id,
                            (p, c) => new ProductInfo
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Catid = p.Catid,
                                CategoryName = c.Name,
                                Slug = p.Slug,
                                Img = p.Img,
                                Detail = p.Detail,
                                Number = p.Number,
                                Price = p.Price,
                                PriceSale = p.PriceSale,
                                Metakey = p.Metakey,
                                MetaDesc = p.MetaDesc,
                                Created_By = p.Created_By,
                                Created_At = p.Created_At,
                                Updated_By = p.Updated_By,
                                Updated_At = p.Updated_At,
                                Status = p.Status,
                            }
                            ).ToList();
                        break;
                    }
            }
            return list;
        }
        public Product getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }
        public Product getRow(string slug)
        {
            return db.Products.Where(m=>m.Slug==slug && m.Status == 1).FirstOrDefault();
        }
        public int Insert(Product row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }
        // Cập nhật mẫu tin
        public int Update(Product row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xó mẫu tin
        public int Delete(Product row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}
