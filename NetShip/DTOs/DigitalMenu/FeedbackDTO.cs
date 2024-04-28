namespace NetShip.DTOs.DigitalMenu
{
    public class FeedbackDTO
    {
        public int Score { get; set; }
        public string Comment { get; set; }
        public Guid BranchId { get; set; }
        public string SessionId { get; set; }
    }

}
