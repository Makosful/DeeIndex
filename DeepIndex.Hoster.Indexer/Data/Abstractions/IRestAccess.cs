using System.Collections.Generic;
using System.IO;

namespace DeepIndex.Hoster.Indexer.Data.Abstractions
{
    public interface IRestAccess
    {
        bool SendBatch(FileInfo fileInfo, Dictionary<string, int> occurrences);
    }
}