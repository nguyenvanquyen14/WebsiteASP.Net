using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    [Table("Categorys")]
    public class Category
    {
        [Key]
        public int Id {get;set;}
        [Required]
        [Display(Name ="Tên danh mục")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public int? Orders { get; set; }

        [Required]
        public string MetaDesc { get; set; }
        [Required]
        public string MetaKey { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int Status { get; set; }
    }
}