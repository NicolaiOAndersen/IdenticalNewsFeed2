using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTO
{
    class NewsDTO
    {
        public int NewsId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string HashTags { get; set; }
    }
}

