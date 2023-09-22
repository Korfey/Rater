#nullable disable
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;

namespace Rater.Models
{
    public class Comment
    {
        [SimpleField(IsFacetable = true)]
        public Guid CommentId { get; set; }

        [SimpleField(IsFilterable = true)]
        public int UserId { get; set; }
       
        [SimpleField(IsFacetable=true)]
        public Guid ReviewId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.StandardLucene)]
        public string Text { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Review Review { get; set; }
    }
}
