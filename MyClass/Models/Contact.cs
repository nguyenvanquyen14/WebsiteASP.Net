using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    [Table("Contacts")]

    public partial class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string ReplayDetail { get; set; }


        public string MetaDesc { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int? Status { get; set; }

    }
}
