using System.ComponentModel.DataAnnotations;

namespace justAsk.Models
{
    public class Post
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }


        public string ApplicationUserId { get; set; }
    }
}