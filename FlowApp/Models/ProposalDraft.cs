namespace FlowApp.Models
{
    public class ProposalDraft
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProposalId { get; set; }
        public string UserId { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}