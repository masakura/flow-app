using System.Data.Entity;

namespace FlowApp.Models
{
    public static class ModelExtensions
    {
        public static void Add(this IDbSet<ProposalDraftAction> actions, int draftId, string type)
        {
            actions.Add(new ProposalDraftAction
            {
                DraftId = draftId,
                Type = type
            });
        }
    }
}