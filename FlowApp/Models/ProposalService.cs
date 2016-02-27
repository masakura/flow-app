using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FlowApp.Models
{
    public class ProposalService
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void Create(ProposalDraft draft)
        {
            var userId = GetUserId();

            var proposal = new Proposal
            {
                UserId = userId
            };
            proposal = _db.Proposals.Add(proposal);
            _db.SaveChanges();

            draft.UserId = userId;
            draft.ProposalId = proposal.Id;
            draft = _db.ProposalDrafts.Add(draft);
            _db.SaveChanges();

            var action = _db.ProposalDraftActions.Add(draft.Id, "draft");
            _db.SaveChanges();

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();
        }

        public List<ProposalViewModel> GetDrafts()
        {
            var drafts = from proposal in _db.ProposalCurrentActions
                select proposal.Action.Draft;

            var proposals = from draft in drafts
                select new ProposalViewModel
                {
                    Id = draft.ProposalId,
                    Title = draft.Title
                };

            return proposals.ToList();
        }

        public ProposalDraft GetDraft(int proposalId)
        {
            var draft = _db.ProposalCurrentActions.Find(proposalId).Action.Draft;
            return new ProposalDraft
            {
                Id = draft.ProposalId,
                Title = draft.Title
            };
        }

        public ProposalDraft SaveDraft(ProposalDraft draft)
        {
            var source = _db.ProposalDrafts.Find(draft.Id);
            draft.Id = 0;
            draft.ProposalId = source.ProposalId;
            draft = _db.ProposalDrafts.Add(draft);
            _db.SaveChanges();

            var action = _db.ProposalDraftActions.Add(draft.Id, "draft");
            _db.SaveChanges();

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();

            return draft;
        }

        private static string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}