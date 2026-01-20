using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebBanThucAnNhanh.Models
{
    public class TypeOfFastFood
    {
        [Key]
        public int IdTypeOfFastFood { get; set; }
        public string NameTypeOfFastFood { get; set; }
    }
}