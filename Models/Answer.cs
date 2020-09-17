namespace justAsk.Models
{
    public class Answer
    {
        public int Id { get; set; }

        //Creator
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //Master post
        public int PostId { get; set; }
        public Post Post { get; set; }

        //Content
        public string Content { get; set; }

        //Karma
        public int Karma { get; set; }
    }
}