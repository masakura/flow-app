using System.Net;
using System.Web.Mvc;
using FlowApp.Models;

namespace FlowApp.Controllers
{
    public class ProposalsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ProposalService _proposalService = new ProposalService();

        // GET: Proposals
        public ActionResult Index()
        {
            return View(_proposalService.GetAll());
        }

        public ActionResult Drafts()
        {
            return View(_proposalService.GetDrafts());
        }

        public ActionResult Requestings()
        {
            return View(_proposalService.GetRequestings());
        }

        public ActionResult YourPublishes()
        {
            return View(_proposalService.GetYourPublishes());
        }

        public ActionResult YourEnds()
        {
            return View(_proposalService.GetYourEnds());
        }

        public ActionResult Pendings()
        {
            return View(_proposalService.GetPendings());
        }

        public ActionResult Publishes()
        {
            return View(_proposalService.GetPublishes());
        }

        public ActionResult Ends()
        {
            return View(_proposalService.GetEnds());
        }

        // GET: Proposals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposal proposal = _db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            return View(proposal);
        }

        // GET: Proposals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proposals/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title")] ProposalDraft draft)
        {
            if (ModelState.IsValid)
            {
                _proposalService.Create(draft);

                return RedirectToAction("Index");
            }

            return View(draft);
        }

        // GET: Proposals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var draft = _proposalService.GetProposal(id.Value);

            if (draft == null)
            {
                return HttpNotFound();
            }

            return View(draft);
        }

        // POST: Proposals/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Action")] ProposalViewModel draft)
        {
            if (ModelState.IsValid)
            {
                switch (draft.Action)
                {
                    case "Save":
                        _proposalService.SaveDraft(new ProposalDraft
                        {
                            ProposalId = draft.Id,
                            Title = draft.Title
                        });
                        break;

                    case "Reject":
                        _proposalService.Reject(draft.Id);
                        break;

                    case "Cancel":
                        _proposalService.Cancel(draft.Id);
                        break;

                    case "Request/Publish":
                        _proposalService.RequestPublish(draft.Id);
                        break;

                    case "Request/End":
                        _proposalService.RequestEnd(draft.Id);
                        break;

                    case "Approve":
                        _proposalService.Approval(draft.Id);
                        break;
                }

                return RedirectToAction("Index");
            }
            return View(draft);
        }

        // GET: Proposals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposal proposal = _db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            return View(proposal);
        }

        // POST: Proposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proposal proposal = _db.Proposals.Find(id);
            _db.Proposals.Remove(proposal);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}