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

        #region 投稿者向け投稿取得

        public List<ProposalViewModel> GetDrafts()
        {
            return _db.ProposalCurrentActions
                .Mine()
                .Types("draft", "cancel", "reject", "cancel/publish", "cancel/end", "reject/publish", "reject/end")
                .ToProposalViewModels()
                .ToList();
        }

        #endregion

        #region 承認者向け投稿取得

        public List<ProposalViewModel> GetAll()
        {
            return _db.ProposalCurrentActions.ToProposalViewModels().ToList();
        }

        public List<ProposalViewModel> GetPendings()
        {
            return _db.ProposalCurrentActions
                .Types("request/publish", "request/end")
                .ToProposalViewModels()
                .ToList();
        }

        public List<ProposalViewModel> GetPublishes()
        {
            var query = from pa in _db.ProposalArticles
                join current in _db.ProposalCurrentActions on pa.ProposalId equals current.ProposalId
                where pa.Article.Displayed
                select current;

            return query.ToProposalViewModels().ToList();
        }

        public List<ProposalViewModel> GetEnds()
        {
            var query = from pa in _db.ProposalArticles
                join current in _db.ProposalCurrentActions on pa.ProposalId equals current.ProposalId
                where !pa.Article.Displayed
                select current;

            return query.ToProposalViewModels().ToList();
        }

        #endregion

        #region  投稿者向け状態変更

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

        public void Cancel(int proposalId)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);
            var newStatus = "cancel";

            switch (current.Action.Type)
            {
                case "request/publish":
                    newStatus = "cancel/publish";
                    break;

                case "request/end":
                    newStatus = "cancel/end";
                    break;
            }

            ChangeStatus(proposalId, newStatus);
        }

        public void Request(int proposalId)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);
            var newStatus = "request/publish";

            switch (current.Action.Type)
            {
                case "draft":
                case "cancel/publish":
                case "reject/publish":
                    newStatus = "request/publish";
                    break;

                case "cancel/end":
                case "reject/end":
                    newStatus = "request/end";
                    break;
            }

            ChangeStatus(proposalId, newStatus);
        }

        public void RequestPublish(int proposalId)
        {
            ChangeStatus(proposalId, "request/publish");
        }

        public void RequestEnd(int proposalId)
        {
            ChangeStatus(proposalId, "request/end");
        }

        #endregion

        #region 承認者向け状態変更

        public void Reject(int proposalId)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);
            var newStatus = "reject";

            switch (current.Action.Type)
            {
                case "request/publish":
                    newStatus = "reject/publish";
                    break;

                case "request/end":
                    newStatus = "reject/end";
                    break;
            }

            ChangeStatus(proposalId, newStatus);
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

        #endregion
    }
}