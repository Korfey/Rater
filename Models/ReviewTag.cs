#nullable disable
namespace Rater.Models
{
    public class ReviewTag
    {
        public Guid ReviewId { get; set; }
        public int TagId { get; set; }

        public Review Review { get; set; }
        public Tag Tag { get; set; }
    }
}
