using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace UploadAndResize
{
    public static class UploadAndResize
    {
        [FunctionName("UploadAndResize")]
        public static async Task<bool> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            HttpRequest req)
        {
            await context.CallActivityAsync<string>(nameof(UploadToBlobAsync), req);

            return true;
        }

        [FunctionName(nameof(UploadToBlobAsync))]
        public static async Task<bool> UploadToBlobAsync([ActivityTrigger] HttpRequest req, ILogger log)
        {
            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            Stream myBlob = new MemoryStream();
            var file = req.Form.Files["File"];
            myBlob = file.OpenReadStream();
            var blobClient = new BlobContainerClient(Connection, containerName);
            var blob = blobClient.GetBlobClient(file.FileName);
            await blob.UploadAsync(myBlob);

            return true;
        }

        [FunctionName("UploadAndResize_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("UploadAndResize", req);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}