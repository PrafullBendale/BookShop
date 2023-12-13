﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name ="List Price ")]
        [Range(0,1000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50 ")]
        [Range(0, 1000, ErrorMessage = "Price must be between 1 to 1000")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(0, 1000, ErrorMessage = "Price must be between 1 to 1000")]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(0, 1000, ErrorMessage = "Price must be between 1 to 1000")]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }   //assigning foreign key
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
