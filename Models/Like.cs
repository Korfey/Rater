#nullable disable
namespace Rater.Models
{
    public class Like
    {
        public int UserId { get; set; }
        public Guid ReviewId { get; set; }

        public User User { get; set; }
        public Review Review { get; set; }
    }
}
