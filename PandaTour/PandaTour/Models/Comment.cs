namespace PandaTour.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int TicketId { get; set; }

        public User User { get; set; }
        public Ticket Ticket { get; set; }
    }
}
