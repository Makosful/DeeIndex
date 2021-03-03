using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepIndex.Hoster.Crawler.Data.Abstractions;
using Microsoft.Extensions.Hosting;

namespace DeepIndex.Hoster.Crawler.Workers
{
    public class Crawler : BackgroundService
    {
        private const string SearchDirectory = @"/home/axl/repository/dls/DeepIndex/data";

        private readonly IRestAccess _restAccess;
        
        public Crawler(IRestAccess restAccess)
        {
            _restAccess = restAccess;
        }

        /// <summary>
        /// The main method called when starting a
        /// <see cref="BackgroundService"/>. Code this worker should run should
        /// go in here. This method is responsible for handling recurring
        /// actions in it's own.  
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var act = new Task(() =>
            {
                // Builds a list of files
                var directoryInfo = new DirectoryInfo(SearchDirectory);
                List<FileInfo> fileInfos = CrawlDirectory(directoryInfo).ToList();

                // Sends the files to a load balancer
                _restAccess.SendFilePaths(fileInfos);
            });

            // Start the task before returning it
            act.Start();
            return act;
        }
        
        /// <summary>
        /// Crawls through the given directory and it's subdirectories,
        /// building a list of files.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private IEnumerable<FileInfo> CrawlDirectory(DirectoryInfo dir)
        {
            // Get all the files in the current directory
            foreach (var file in dir.EnumerateFiles()) yield return file;

            // Go through all the directories
            foreach (var directory in dir.EnumerateDirectories())
            {
                // And return the files in those directories
                foreach (var file in CrawlDirectory(directory)) yield return file;
            }
        }
    }
}
