#nullable disable
namespace Rater.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
