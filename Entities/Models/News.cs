using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
   public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required]
        public string Author { get; set; }
        //I choose to comment out the lenghts for testing purposes.
        [Required]
        [MinLength(10)]
        public string Title { get; set; }
        [Required]
        [MinLength(50)]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        public string HashTags { get; set; }

        //Optimistic concurrency, not done.
        [Timestamp]
        public Byte[] version { get; set; }

    }
}
