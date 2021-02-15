using System.Collections.Generic;
using System.IO;
using DeepIndex.Core.Entities;
using DeepIndex.Core.ApplicationServices.Abstractions;

namespace DeepIndex.Core.DomainServices
{
    public interface IOccurrenceDomain 
    {
        IEnumerable<Occurrence> SearchTerm(string input);
        
        Occurrence AddIndex(Occurrence occurrence);

        bool AddBatch(IEnumerable<Occurrence> occurrences);
    }
}