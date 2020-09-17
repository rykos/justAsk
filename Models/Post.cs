using System.Collections.Generic;
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

        //Creator
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //Answers
        public List<Answer> Answers { get; set; }

        //Karma
        public int Karma { get; set; }
    }
}