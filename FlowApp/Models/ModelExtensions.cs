﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FlowApp.Models
{
    public static class ModelExtensions
    {
        public static ProposalDraftAction Add(this IDbSet<ProposalDraftAction> actions, ProposalDraft draft, string type,
            ApprovalStatus approvalStatus = ApprovalStatus.None)
        {
            return actions.Add(new ProposalDraftAction
            {
                DraftId = draft.Id,
                Draft = draft,
                Type = type,
                ApprovalStatus = approvalStatus
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

        public static IQueryable<ProposalCurrentAction> Mine(this IQueryable<ProposalCurrentAction> currents)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return currents.Where(current => current.Proposal.UserId == userId || current.Action.Draft.UserId == userId);
        }

        public static IQueryable<ProposalCurrentAction> Type(this IQueryable<ProposalCurrentAction> currents,
            string type)
        {
            return currents.Where(current => current.Action.Type == type);
        }

        public static IQueryable<ProposalCurrentAction> Types(this IQueryable<ProposalCurrentAction> currents,
            params string[] types)
        {
            return from current in currents
                join type in types on current.Action.Type equals type
                select current;
        }

        public static IQueryable<ProposalCurrentAction> ApprovalStatuses(
            this IQueryable<ProposalCurrentAction> currents, params ApprovalStatus[] statuses)
        {
            return from current in currents
                join status in statuses on current.Action.ApprovalStatus equals status
                select current;
        }

        public static IEnumerable<ProposalViewModel> ToProposalViewModels(
            this IEnumerable<ProposalCurrentAction> currents)
        {
            return ProposalViewModel.Create(currents);
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