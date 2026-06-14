using System.ComponentModel.DataAnnotations;
using System;

namespace InfoPublishSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "User";

        public DateTime RegisterTime { get; set; } = DateTime.Now;
    }
}
