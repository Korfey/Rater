using Microsoft.AspNetCore.Identity;
#nullable disable

namespace Rater.Models
{
    public class User: IdentityUser<int>
    {
        public override string UserName { get; set; }
        public ICollection<Comment> Comments { get; } = new HashSet<Comment>();
        public ICollection<Rating> Ratings { get; } = new HashSet<Rating>();
        public ICollection<Like> Likes { get; } = new HashSet<Like>();
        public ICollection<Review> Reviews { get; } = new List<Review>();
    }
}
