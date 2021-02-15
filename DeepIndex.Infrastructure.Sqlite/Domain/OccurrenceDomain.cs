using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeepIndex.Core.DomainServices;
using DeepIndex.Core.Entities;

namespace DeepIndex.Infrastructure.Sqlite.Domain
{
    public class OccurrenceDomain: IOccurrenceDomain
    {
        private readonly IndexContext _context;

        public OccurrenceDomain(IndexContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public Occurrence AddIndex(Occurrence occurrence)
        {
            var entity = _context.Occurrences.Add(occurrence).Entity;
            _context.SaveChanges();
            return entity;
        }

        public bool AddBatch(IEnumerable<Occurrence> occurrences)
        {
            _context.Occurrences.AddRange(occurrences);
            var changes = _context.SaveChanges();
            return changes > 0;
        }
        
        IEnumerable<Occurrence> IOccurrenceDomain.SearchTerm(string input)
        {
            var queryable = from occurrence in _context.Occurrences
                where occurrence.Term == input
                orderby occurrence.Count descending 
                select occurrence;

            return queryable.ToList();
        }
    }
}