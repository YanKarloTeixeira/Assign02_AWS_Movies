using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assign02_AWS_Movies.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required (ErrorMessage = "The category's name is required")]
        [StringLength(30, ErrorMessage = "The category's size must be under 30 characters.")]
        public string CategoryName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }


    }
}
