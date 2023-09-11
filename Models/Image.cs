#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rater.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public int ReviewId { get; set; }
        public string ImageUrl { get; set; }

        public Review Review { get; set; }
    }
}
