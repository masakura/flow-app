using System.ComponentModel.DataAnnotations;

namespace FlowApp.Models
{
    public class Article
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Title { get; set; }
    }
}