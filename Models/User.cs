using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    
namespace Rater.Models
{
    public class User: IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ICollection<Comment> Comments { get; } = new HashSet<Comment>();
        public ICollection<Rating> Ratings { get; } = new HashSet<Rating>();
        public ICollection<Like> Likes { get; } = new HashSet<Like>();
        public ICollection<Review> Reviews { get; } = new List<Review>();
    }
}
