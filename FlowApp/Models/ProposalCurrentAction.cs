using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowApp.Models
{
    public class ProposalCurrentAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProposalId { get; set; }

        public int ActionId { get; set; }
        public virtual Proposal Proposal { get; set; }
        public virtual ProposalDraftAction Action { get; set; }
    }
}