using Common.Interfaces;
using Enums.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuoteData.Repositories
{
    public interface IQuoteRepository
    {
        IQuote Add(IQuote quote);
        Task<IQuote> GetAsync(int id);
    }
}
