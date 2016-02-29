using System.Data.Entity;

namespace FlowApp.Models
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public void Save(int proposalId, bool displayed)
        {
            var current = _db.ProposalCurrentActions.Find(proposalId);
            var proposalArticle = _db.ProposalArticles.Find(proposalId);

            if (proposalArticle != null)
            {
                var article = _db.Articles.Find(proposalArticle.ArticleId);
                article.Title = current.Action.Draft.Title;
                article.Displayed = displayed;
                _db.Entry(proposalArticle).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                var article = new Article
                {
                    Title = current.Action.Draft.Title,
                    Displayed = displayed
                };
                article = _db.Articles.Add(article);
                _db.SaveChanges();

                _db.ProposalArticles.Add(new ProposalArticle {ProposalId = proposalId, ArticleId = article.Id});
                _db.SaveChanges();
            }
        }
    }
}