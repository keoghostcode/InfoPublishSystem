using System;

namespace InfoPublishSystem.Models.ViewModels
{
    public class NewsDetailViewModel
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? PublishTime { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
    }
}