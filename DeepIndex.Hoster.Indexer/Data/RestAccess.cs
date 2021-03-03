using System.Collections.Generic;
using System.IO;
using DeepIndex.Hoster.Indexer.Data.Abstractions;
using RestSharp;

namespace DeepIndex.Hoster.Indexer.Data
{
    public class RestAccess : IRestAccess
    {
        private const string BaseUrl = "";
        private readonly RestClient _restClient;

        public RestAccess()
        {
            _restClient = new RestClient(BaseUrl);
        }

        public bool SendBatch(FileInfo fileInfo, Dictionary<string, int> occurrences)
        {
            var request = new RestRequest("occurrences")
                .AddJsonBody(occurrences);

            IRestResponse response = _restClient.Post(request);

            return response.IsSuccessful;
        }
    }
}