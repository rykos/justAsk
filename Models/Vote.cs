namespace justAsk.Models
{
    public class Vote
    {
        public int Id { get; set; }

        //Creator
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //Post | Answer id
        public int ContentId { get; set; }

        public VoteState State { get; set; }
    }

    public enum VoteState
    {
        plus,
        minus
    }
}