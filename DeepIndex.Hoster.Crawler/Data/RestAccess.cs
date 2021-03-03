using System.Collections.Generic;
using System.IO;
using DeepIndex.Hoster.Crawler.Data.Abstractions;
using RestSharp;

namespace DeepIndex.Hoster.Crawler.Data
{
    public class RestAccess : IRestAccess
    {
        private const string BaseUrl = "";
        private const string Resource = "paths";

        private readonly RestClient _rest;
        
        public RestAccess()
        {
            _rest = new RestClient(BaseUrl);
        }

        /// <summary>
        /// Sends the list of files to the load balancer
        /// </summary>
        /// <param name="fileInfos"></param>
        public void SendFilePaths(IEnumerable<FileInfo> fileInfos)
        {
            var request = new RestRequest(Resource, DataFormat.Json)
                .AddJsonBody(fileInfos);
            IRestResponse response = _rest.Post(request);

            if (response.IsSuccessful)
            {
                // Success
            }
            else
            {
                // Fail
            }
        }
    }
}