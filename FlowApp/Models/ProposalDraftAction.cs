namespace FlowApp.Models
{
    public class ProposalDraftAction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int DraftId { get; set; }
        public virtual ProposalDraft Draft { get; set; }
    }
}