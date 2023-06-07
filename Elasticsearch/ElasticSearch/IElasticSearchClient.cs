namespace Elasticsearch.ElasticSearch
{
    public interface IElasticSearchClient
    {
        Task<string> SearchAsync(string indexName, string keyword);

        Task<bool> AddIndexAsync(object dataToIndex, string indexName, string id);
    }
}
