using System;
using System.ComponentModel.DataAnnotations;

namespace InfoPublishSystem.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int NewsId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public int? ParentCommentId { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
    }
}
