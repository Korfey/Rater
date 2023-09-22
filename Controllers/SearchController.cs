using Microsoft.AspNetCore.Mvc;
using Rater.Models;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Identity;

namespace Rater.Controllers
{
    public class SearchController: Controller
    {
        private static SearchIndexClient indexClient;

        private static SearchClient searchClient;

        [HttpPost]
        public async Task<ActionResult> Index(SearchData model)
        {
            try
            {
                model.searchText ??= "";
                await RunQueryAsync(model);
            }

            catch
            {
                return View("Error", new ErrorViewModel { RequestId = "1" });
            }
            return View(model);
        }

        private async Task<ActionResult> RunQueryAsync(SearchData model)
        {
            var options = new SearchOptions()
            {
                IncludeTotalCount = true
            };
            AddReturnProperties(options);

            model.resultList = await searchClient
                .SearchAsync<Review>(model.searchText, options)
                .ConfigureAwait(false);

            return PartialView("~Views/Shared/SearchPartial", model);
        }

        private void AddReturnProperties(SearchOptions options)
        {
            options.Select.Add("Title");
            options.Select.Add("Content");
        }

        public static void Initialize(string searchServiceUri, string queryApiKey, string indexName)
        {
            var serviceEndpoint = new Uri(searchServiceUri);

            var credential = new AzureKeyCredential(queryApiKey);

            indexClient = new SearchIndexClient(serviceEndpoint, credential);
            searchClient = indexClient.GetSearchClient(indexName);
        }
    }
}
