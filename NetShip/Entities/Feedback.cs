namespace NetShip.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public Guid BranchId { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Branch Branch { get; set; }
    }

}
