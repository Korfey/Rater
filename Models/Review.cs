#nullable disable
namespace Rater.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public byte Rating { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }

        public ICollection<Image> Images { get; } = new List<Image>();
        public ICollection<Tag> Tags { get; } = new List<Tag>();
        public ICollection<Comment> Comments { get; } = new List<Comment>();
        public ICollection<Like> Likes { get; } = new HashSet<Like>(); 
        public ICollection<Rating> Ratings { get; } = new HashSet<Rating>();
    }
}
