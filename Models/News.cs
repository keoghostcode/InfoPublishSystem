using System;
using System.ComponentModel.DataAnnotations;


namespace InfoPublishSystem.Models
{
    public class News
    {
        public int NewsId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public DateTime PublishTime { get; set; } = DateTime.Now;

        public bool IsVisible { get; set; } = true;
    }
}
