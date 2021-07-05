using Dapr.Actors;
using System.Threading.Tasks;

namespace MinimalApiActor
{
    public interface IBankActor : IActor
    {
        Task<AccountBalance> GetAccountBalance();

        Task Withdraw(decimal amount);
    }
}