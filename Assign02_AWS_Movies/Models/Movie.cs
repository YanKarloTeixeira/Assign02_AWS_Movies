using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assign02_AWS_Movies.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        [Required (ErrorMessage ="The movie's title is required")]
        [StringLength(150, ErrorMessage = "The comment's size must be under 150 characters.")]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength = 20, ErrorMessage = "The movie's description size must be between 20 to 1000 characters.")]
        public string description { get; set; }
        public DateTime? UploadDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "The movie's file name is required")]
        [StringLength(100, ErrorMessage = "The file's name size must be under 100 characters.")]
        public string FileName { get; set; }
        public int Downloads { get; set; }
        [StringLength(256)]
        public string User { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
