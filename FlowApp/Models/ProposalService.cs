using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FlowApp.Models
{
    public class ProposalService
    {
        private readonly ArticleService _articleService = new ArticleService();
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

            var action = _db.ProposalDraftActions.Add(draft, "draft");
            _db.SaveChanges();

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();
        }

        public List<ProposalViewModel> GetAll()
        {
            var proposals = from current in _db.ProposalCurrentActions
                select new ProposalViewModel
                {
                    Id = current.ProposalId,
                    Title = current.Action.Draft.Title,
                    Status = current.Action.Type
                };

            return proposals.ToList();
        }

        public List<ProposalViewModel> GetDrafts()
        {
            var userId = GetUserId();

            var proposals = from current in _db.ProposalCurrentActions
                where current.Proposal.UserId == userId || current.Action.Draft.UserId == userId
                select new ProposalViewModel
                {
                    Id = current.ProposalId,
                    Title = current.Action.Draft.Title,
                    Status = current.Action.Type,
                    ProposalUser = new UserViewModel
                    {
                        Id = current.Proposal.UserId,
                        UserName = current.Proposal.User.UserName
                    },
                    DraftUser = new UserViewModel
                    {
                        Id = current.Action.Draft.UserId,
                        UserName = current.Action.Draft.User.UserName
                    }
                };

            return proposals.ToList();
        }

        public ProposalViewModel GetProposal(int proposalId)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);
            return new ProposalViewModel
            {
                Id = current.ProposalId,
                Title = current.Action.Draft.Title,
                Status = current.Action.Type
            };
        }

        public ProposalDraft SaveDraft(ProposalDraft draft)
        {
            draft.Id = 0;
            draft = _db.ProposalDrafts.Add(draft);
            _db.SaveChanges();

            var action = _db.ProposalDraftActions.Add(draft, "draft");
            _db.SaveChanges();

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();

            return draft;
        }

        public void ToWaiting(int proposalId)
        {
            ChangeStatus(proposalId, "waiting");
        }

        public void ToDraft(int proposalId)
        {
            ChangeStatus(proposalId, "draft");
        }

        public void ToShown(int proposalId)
        {
            ChangeStatus(proposalId, "shown");

            _articleService.Show(proposalId);
        }

        private void ChangeStatus(int proposalId, string status)
        {
            var prevAction = _db.ProposalCurrentActions.Find(proposalId).Action;

            var action = _db.ProposalDraftActions.Add(prevAction.Draft, status);
            _db.SaveChanges();
            action = _db.ProposalDraftActions.Find(action.Id);

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();
        }

        private static string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}