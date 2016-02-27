namespace FlowApp.Models
{
    public class ProposalDraft
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProposalId { get; set; }
        public string UserId { get; set; }
        public Proposal Proposal { get; set; }
        public ApplicationUser User { get; set; }
    }
}