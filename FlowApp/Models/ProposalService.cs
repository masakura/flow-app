﻿using System.Collections.Generic;
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
            _db.Proposals.Add(proposal);

            draft.UserId = userId;
            _db.ProposalDrafts.Add(draft);

            _db.SaveChanges();
        }

        public IList<ProposalDraft> GetDrafts()
        {
            return _db.ProposalDrafts.ToList();
        }

        public ProposalDraft GetDraft(int id)
        {
            return _db.ProposalDrafts.Find(id);
        }

        public ProposalDraft SaveDraft(ProposalDraft draft)
        {
            var source = _db.ProposalDrafts.Find(draft.Id);
            draft.Id = 0;
            draft.ProposalId = source.ProposalId;
            draft = _db.ProposalDrafts.Add(draft);

            _db.SaveChanges();

            return draft;
        }

        private static string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}