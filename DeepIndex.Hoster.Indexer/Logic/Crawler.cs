using System;
using System.Collections.Generic;
using System.IO;
using DeepIndex.Hoster.Indexer.Data.Abstractions;
using DeepIndex.Hoster.Indexer.Logic.Abstractions;

namespace DeepIndex.Hoster.Indexer.Logic
{
    public class Crawler : ICrawler
    {
        private readonly IRestAccess _restAccess;

        public Crawler(IRestAccess restAccess)
        {
            _restAccess = restAccess;
        }

        /// <summary>
        /// Crawls through a file, counts the words and then saves it to a database
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ICrawler.CrawlFile(string path)
        {
            // Loads the whole file into memory
            var fileInfo = new FileInfo(path);
            using var reader = new StreamReader(fileInfo.FullName);
            
            string content = reader.ReadToEnd();
            // Collapses the content into a single line
            content = content.Replace(Environment.NewLine, " ");
            content = content.Replace("-", " ");
            content = content.Replace("  ", " ");
            string[] split = content.Split(' ');
            Dictionary<string,int> occurrences = CountOccurrences(split);
            
            bool success = _restAccess.SendBatch(fileInfo, occurrences);
            return success;
        }
        
        /// <summary>
        /// Loops through the array and counts the occurrences of every word,
        /// then returns a dictionary with each word, key, matched with the
        /// count, value.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static Dictionary<string, int> CountOccurrences(IEnumerable<string> arr)
        {
            var occurrence = new Dictionary<string, int>();
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