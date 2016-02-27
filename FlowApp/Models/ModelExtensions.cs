using System.Data.Entity;

namespace FlowApp.Models
{
    public static class ModelExtensions
    {
        public static ProposalDraftAction Add(this IDbSet<ProposalDraftAction> actions, int draftId, string type)
        {
            return actions.Add(new ProposalDraftAction
            {
                DraftId = draftId,
                Type = type
            });
        }

        public static ProposalCurrentAction AddOrUpdate(this IDbSet<ProposalCurrentAction> actions,
            ProposalDraftAction action)
        {
            var currentAction = actions.Find(action.Draft.ProposalId) ??
                                actions.Add(new ProposalCurrentAction {ProposalId = action.Draft.ProposalId});
            currentAction.ActionId = action.Id;

            return currentAction;
        }

        private static ProposalCurrentAction CurrentAction(int proposalId, int actionId)
        {
            return new ProposalCurrentAction
            {
                ProposalId = proposalId,
                ActionId = actionId
            };
        }

        private static ProposalCurrentAction CurrentAction(ProposalDraftAction action)
        {
            return CurrentAction(action.Draft.ProposalId, action.Id);
        }
    }
}