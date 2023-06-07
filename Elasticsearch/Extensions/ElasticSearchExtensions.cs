using Elasticsearch.Net;

namespace Elasticsearch.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var uris = new[]
            {
                new Uri(configuration["elasticsearch:url"])
            };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionConfiguration(connectionPool);
            var client = new ElasticLowLevelClient(settings);

            services.AddSingleton<IElasticLowLevelClient>(client);
        }
    }
}
