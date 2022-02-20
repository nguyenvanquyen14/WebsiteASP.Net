using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    [Table("Users")]

    public partial class User
    {
        public int Id { get; set; }

        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? Access { get; set; }
        public int? CountError { get; set; }




        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int? Status { get; set; }
        public string Roles { get; set; }
        public string Img { get; set; }       
    }
}
