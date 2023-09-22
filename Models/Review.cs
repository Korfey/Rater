#nullable disable
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rater.Models
{
    public class Review
    {
        [SimpleField(IsFilterable = true, IsKey = true)]
        public Guid ReviewId { get; set; }

        [SimpleField(IsFilterable = true)]
        public int UserId { get; set; }

        [SimpleField(IsFilterable = true)]
        public int ProductId { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public byte Rating { get; set; }

        [SearchableField(IsSortable = true)]
        public string Title { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.StandardLucene)]
        public string Content { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTime Created { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public Group Group { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }

        public ICollection<Image> Images { get; } = new List<Image>();
        public ICollection<Tag> Tags { get; } = new List<Tag>();
        public ICollection<Comment> Comments { get; } = new List<Comment>();
        public ICollection<Like> Likes { get; } = new HashSet<Like>(); 
        public ICollection<Rating> Ratings { get; } = new HashSet<Rating>();
    }

    public enum Group
    {
        Movie,
        Game, 
        Book
    }
}
