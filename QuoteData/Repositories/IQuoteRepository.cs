using Common.Interfaces;
using System.Threading.Tasks;

namespace QuoteData.Repositories
{
    public interface IQuoteRepository
    {
        IQuote Add(IQuote quote);
        Task<IQuote> GetAsync(int id);
    }
}
