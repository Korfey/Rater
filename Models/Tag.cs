namespace Rater.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public ICollection<Review> Reviews { get; } = new HashSet<Review>();
    }
}
