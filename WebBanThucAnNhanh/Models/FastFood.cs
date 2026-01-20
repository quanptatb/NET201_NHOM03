using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanThucAnNhanh.Models
{
    public class FastFood
    {
        [Key]
        public int IdFastFood { get; set; }
        public string NameFastFood { get; set; }
        public double Price { get; set; }        
        public int Quantity { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public int IdTypeOfFastFood { get; set; }
        public TypeOfFastFood TypeOfFastFood { get; set; }
    }
}