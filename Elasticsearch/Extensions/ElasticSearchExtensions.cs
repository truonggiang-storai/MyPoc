using Elasticsearch.Net;

namespace Elasticsearch.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            //var uris = new[]
            //{
            //    new Uri(configuration["elasticsearch:url"])
            //};
            //var connectionPool = new SniffingConnectionPool(uris);
            //var settings = new ConnectionConfiguration(connectionPool);

            var settings = new ConnectionConfiguration(new Uri("https://localhost:9200"))
                .CertificateFingerprint("08a638f2d5a67c94d86bab85f5e3d55f3dda3962b437b6c7145216c12ba23c1e")
                .BasicAuthentication("elastic", "ap2wwcL=Jr0BhBSDwoZY");
            var client = new ElasticLowLevelClient(settings);

            services.AddSingleton<IElasticLowLevelClient>(client);
        }
    }
}
