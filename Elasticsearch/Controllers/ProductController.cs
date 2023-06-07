using Elasticsearch.ElasticSearch;
using Elasticsearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IElasticSearchClient _elasticClient;

        private readonly IConfiguration _configuration;

        public ProductController(IElasticSearchClient elasticClient, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _configuration = configuration;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword;
            var index = _configuration["elasticsearch:index"];

            var result = await _elasticClient.SearchAsync(index, keyword);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddIndexAsync([FromBody] Product product)
        {
            var index = _configuration["elasticsearch:index"];

            var result = await _elasticClient.AddIndexAsync(product, index, product.Id.ToString());

            return Ok(result);
        }
    }
}
