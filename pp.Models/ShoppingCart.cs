using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp.Models
{
    public  class ShoppingCart
    {
        public int Id { get; set; } 
        public int ProductId { get; set;}
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(0,1000, ErrorMessage ="Quantity must be betwwen 1 to 1000")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }
        [NotMapped] 
        public double Price { get; set;}
    }
}
