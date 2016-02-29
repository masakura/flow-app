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
            return _db.ProposalCurrentActions.ToProposalViewModels().ToList();
        }

        public List<ProposalViewModel> GetDrafts()
        {
            return _db.ProposalCurrentActions
                .Mine()
                .Type("draft")
                .ToProposalViewModels()
                .ToList();
        }

        public List<ProposalViewModel> GetPendings()
        {
            return _db.ProposalCurrentActions
                .Types("request/publish", "request/end")
                .ToProposalViewModels()
                .ToList();
        }

        public List<ProposalViewModel> GetShowns()
        {
            return _db.ProposalCurrentActions
                .Type("shown")
                .ToProposalViewModels()
                .ToList();
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

        public void ToDraft(int proposalId)
        {
            ChangeStatus(proposalId, "draft");
        }

        public void Approval(int proposalId)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);

            var action = current.Action;
            switch (action.Type)
            {
                case "request/publish":
                    ChangeStatus(proposalId, "publish");
                    break;

                case "request/end":
                    ChangeStatus(proposalId, "end");
                    break;
            }
        }

        public void RequestPublish(int proposalId)
        {
            ChangeStatus(proposalId, "request/publish");
        }

        public void RequestEnd(int proposalId)
        {
            ChangeStatus(proposalId, "request/end");
        }

        private void ChangeStatus(int proposalId, string status)
        {
            var prevAction = _db.ProposalCurrentActions.Find(proposalId).Action;

            var action = _db.ProposalDraftActions.Add(prevAction.Draft, status);
            _db.SaveChanges();
            action = _db.ProposalDraftActions.Find(action.Id);

            _db.ProposalCurrentActions.AddOrUpdate(action);
            _db.SaveChanges();

            switch (status)
            {
                case "publish":
                    _articleService.Save(proposalId, true);
                    break;
                case "end":
                    _articleService.Save(proposalId, false);
                    break;
            }
        }

        private static string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}