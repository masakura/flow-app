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
            _db.Proposals.Add(proposal);

            draft.UserId = userId;
            _db.ProposalDrafts.Add(draft);

            _db.SaveChanges();
        }

        public IList<ProposalDraft> GetAll()
        {
            return _db.ProposalDrafts.ToList();
        }

        private static string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}