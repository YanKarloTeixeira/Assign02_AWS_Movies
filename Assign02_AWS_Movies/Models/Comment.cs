using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assign02_AWS_Movies.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        
        [StringLength(256)]
        public string User { get; set; }

        [Required(ErrorMessage = "The movie's comment about is required")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "The comment's characters amount must be between 20 to 1000 characters.")]
        public string CommentText { get; set; }

        [Required(ErrorMessage = "The movie's rate is required")]
        [Range(1, 10, ErrorMessage = "The rate must be between 1 and 100")]
        public int value { get; set; }
        public DateTime? CommentDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
