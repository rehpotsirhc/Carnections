﻿using Common.Interfaces;
using System.Threading.Tasks;

namespace QuoteData.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {

        QuoteDbContext _dbContext;

        public QuoteRepository(QuoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQuote Add(IQuote quote)
        {
            return _dbContext.Set<IQuote>().Add(quote).Entity;
        }

        public async Task<IQuote> GetAsync(int id)
        {
            return await _dbContext.Set<IQuote>().FindAsync(id);
        }
    }
}
