using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    [Table("Orderdetails")]

    public partial class Orderdetail
    {
        public int Id { get; set; }

        
        public int? Orderid { get; set; }
        public int Productid { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }


    }
}
