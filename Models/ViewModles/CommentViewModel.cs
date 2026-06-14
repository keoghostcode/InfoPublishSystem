using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfoPublishSystem.Models.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }

        public int NewsId { get; set; }

        [Required]
        [Display(Name = "留言内容")]
        [StringLength(500)]
        public string Content { get; set; }

        public int? ParentCommentId { get; set; }

        public string NewsTitle { get; set; }

        public string UserName { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsApproved { get; set; }

        public List<CommentViewModel> Children { get; set; } = new List<CommentViewModel>();
    }
}
