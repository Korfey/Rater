#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rater.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int ReviewId { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Review Review { get; set; }
    }
}
