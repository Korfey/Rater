using Azure.Search.Documents.Models;

namespace Rater.Models
{
    public class SearchData
    {
        public string searchText { get; set; }

        public SearchResults<Review> resultList;
    }
}
