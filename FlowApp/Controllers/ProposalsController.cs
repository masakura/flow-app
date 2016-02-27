using System.Data.Entity;
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
            Proposal proposal = _db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            return View(proposal);
        }

        // POST: Proposals/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(proposal).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proposal);
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