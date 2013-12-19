using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEF6.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        public ICollection<Product> Products; 
        public string Name { get; set; }
    }
}
