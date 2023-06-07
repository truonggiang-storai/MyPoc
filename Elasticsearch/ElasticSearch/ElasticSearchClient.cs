using Elasticsearch.Net;
using System.Diagnostics;

namespace Elasticsearch.ElasticSearch
{
    public class ElasticSearchClient : IElasticSearchClient
    {
        private readonly IElasticLowLevelClient _elasticLowLevelClient;

        public ElasticSearchClient(IElasticLowLevelClient elasticLowLevelClient)
        {
            _elasticLowLevelClient = elasticLowLevelClient;
        }

        public async Task<string> SearchAsync(string indexName, string keyword)
        {
            var searchResponse = await _elasticLowLevelClient.SearchAsync<StringResponse>(indexName, PostData.Serializable(new
            {
                from = 0,
                size = 10,
                query = new
                {
                    match = new
                    {
                        name = keyword
                    }
                }
            }));

            var successful = searchResponse.Success;
            return searchResponse.Body;
        }

        public async Task<bool> AddIndexAsync(object dataToIndex, string indexName, string id)
        {
            var response = await _elasticLowLevelClient.IndexAsync<StringResponse>(indexName, id, PostData.Serializable(dataToIndex));

            return response.Success;
        }
    }
}
