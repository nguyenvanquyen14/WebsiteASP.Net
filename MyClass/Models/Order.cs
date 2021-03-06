using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    [Table("Orders")]

    public partial class Order
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public int Userid { get; set; }      
        public string Deliveryaddress { get; set; }
        public string Deliveryname { get; set; }
        public string Deliveryphone { get; set; }
        public string Deliveryemail { get; set; }
        public string Note { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }


        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int? Status { get; set; }       
    }
}