using System.ComponentModel.DataAnnotations;

namespace InfoPublishSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public int SortOrder { get; set; } = 0;
    }
}
