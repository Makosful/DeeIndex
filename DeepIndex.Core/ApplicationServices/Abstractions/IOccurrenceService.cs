using System.Collections.Generic;
using System.IO;
using DeepIndex.Core.Entities;

namespace DeepIndex.Core.ApplicationServices.Abstractions
{
    public interface IOccurrenceService
    {
        public bool CreateBatch(FileInfo fileInfo, Dictionary<string, int> batch);
        
        IEnumerable<Occurrence> SearchTerm(string input);
    }
}