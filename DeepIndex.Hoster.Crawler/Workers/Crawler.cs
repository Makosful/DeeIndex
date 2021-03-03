using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepIndex.Core.ApplicationServices.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeepIndex.Hoster.Crawler.Workers
{
    public class Crawler : BackgroundService
    {
        private const string SearchDirectory = @"/home/axl/repository/dls/DeepIndex/data";
        
        private readonly ILogger<Crawler> _logger;
        private readonly IOccurrenceService _occurrenceService;
        
        public Crawler(ILogger<Crawler> logger, IOccurrenceService occurrenceService)
        {
            _logger = logger;
            _occurrenceService = occurrenceService;
        }

        /// <summary>
        /// Called before <see cref="ExecuteAsync"/>. Used to set up persistent
        /// variables. 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {Worker}", nameof(Crawler));
            
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Called before the worker closes gracefully. Similar to the Dispose
        /// method.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {Worker}", nameof(Crawler));
            
            return base.StopAsync(cancellationToken);
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
            Task act = new Task(() =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(SearchDirectory);
                List<FileInfo> fileInfos = CrawlDirectory(directoryInfo).ToList();

                foreach (var info in fileInfos)
                {
                    //CrawlFile(info);
                }
            });

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
            _logger.LogInformation("Looking in {Directory}", dir);

            Stopwatch s = new Stopwatch();
            s.Start();
            // The 'yield' keyword automatically adds the results to an
            // anonymous IEnumerable that will be returned once the
            // methods hits it's exit point
            foreach (var file in dir.EnumerateFiles()) yield return file;

            foreach (var directory in dir.EnumerateDirectories())
            foreach (var file in CrawlDirectory(directory)) yield return file;
            
            s.Stop();
            Console.WriteLine($"{s.Elapsed}");
        }

        /// <summary>
        /// Browses a single file to index all the words
        /// </summary>
        /// <param name="fileInfo"></param>
        private void CrawlFile(FileInfo fileInfo)
        {
            _logger.LogInformation("Browsing file: {File}", fileInfo.FullName);

            // Loads the whole file into memory
            using StreamReader reader = new StreamReader(fileInfo.FullName);
            string content = reader.ReadToEnd();
            // Collapses the content into a single line
            content = content.Replace(System.Environment.NewLine, " ");
            string[] split = content.Split(' ');
            Dictionary<string,int> occurrences = CountOccurrences(split);
            
            var success = _occurrenceService.CreateBatch(fileInfo, occurrences);
            Console.WriteLine(success);
        }

        /// <summary>
        /// Loops through the array and counts the occurrences of every word,
        /// then returns a dictionary with each word, key, matched with the
        /// count, value.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private Dictionary<string, int> CountOccurrences(string[] arr)
        {
            Dictionary<string, int> occurrence = new Dictionary<string, int>();
            foreach (string s in arr)
            {
                // Gets the current count of the word.
                // 0 if it has not occured yet. 
                int score = occurrence.GetValueOrDefault(s);
                occurrence[s] = score + 1; // Overrides/adds the score +1
            }
            
            return occurrence;
        }
    }
}
