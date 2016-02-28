using System.Collections.Generic;
using System.Linq;

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

        public static ProposalViewModel Create(ProposalCurrentAction current)
        {
            return new ProposalViewModel
            {
                Id = current.ProposalId,
                Title = current.Action.Draft.Title,
                Status = current.Action.Type,
                ProposalUser = UserViewModel.Create(current.Proposal.User),
                DraftUser = UserViewModel.Create(current.Action.Draft.User)
            };
        }

        public static IEnumerable<ProposalViewModel> Create(IEnumerable<ProposalCurrentAction> currents)
        {
            return currents.Select(Create);
        }
    }
}