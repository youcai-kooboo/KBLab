using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEF6.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Published { get; set; }
        public decimal Storage { get; set; }

        public Category Category { get; set; }
    }
}
