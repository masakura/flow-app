using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowApp.Models
{
    public class ProposalArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProposalId { get; set; }

        public int ArticleId { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual Article Article { get; set; }
    }
}