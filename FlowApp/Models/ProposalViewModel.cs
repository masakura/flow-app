namespace FlowApp.Models
{
    public class ProposalViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public UserViewModel ProposalUser { get; set; }
        public UserViewModel DraftUser { get; set; }
    }
}