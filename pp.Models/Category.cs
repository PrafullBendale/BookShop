using System.ComponentModel.DataAnnotations;

namespace pp.Models
{
    public class Category
    {

        //adding category columns
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Name can-not be longer than than 100 characters")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Display Order")]
        [Range(0, 100, ErrorMessage = "Display Order must be between 0 to 100")]
        public int DisplayOrder { get; set; }
    }
}
