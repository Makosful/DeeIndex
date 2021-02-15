using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeepIndex.Core.ApplicationServices.Abstractions;
using DeepIndex.Core.DomainServices;
using DeepIndex.Core.Entities;

namespace DeepIndex.Core.ApplicationServices
{
    public class OccurrenceService : IOccurrenceService
    {
        private readonly IOccurrenceDomain _occurrenceDomain;

        public OccurrenceService(IOccurrenceDomain occurrenceDomain)
        {
            _occurrenceDomain = occurrenceDomain;
        }

        bool IOccurrenceService.CreateBatch(FileInfo fileInfo, Dictionary<string, int> batch)
        {
            Console.WriteLine("Test");
            // foreach (var (name, count) in batch)
            // {
            //     _occurrenceDomain.AddIndex(new Occurrence()
            //     {
            //         File = fileInfo.FullName,
            //         Term = name,
            //         Count = count
            //     });
            // }
            var occurrences = ConvertDictionaryToList(fileInfo, batch);

            return _occurrenceDomain.AddBatch(occurrences);
        }

        IEnumerable<Occurrence> IOccurrenceService.SearchTerm(string input)
        {
            return _occurrenceDomain.SearchTerm(input);
        }

        private static IEnumerable<Occurrence> ConvertDictionaryToList(FileSystemInfo fileInfo, Dictionary<string, int> occurrences)
        {
            foreach (var (key, value) in occurrences)
            {
                yield return new Occurrence()
                {
                    File = fileInfo.FullName,
                    Term = key,
                    Count = value,
                };
            }
        }
    }
}