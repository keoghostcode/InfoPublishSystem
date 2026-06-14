using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfoPublishSystem.Models.ViewModels
{
    public class NewsViewModel
    {
        public int NewsId { get; set; }

        [Required]
        [Display(Name = "标题")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "所属栏目")]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }
    }
}
