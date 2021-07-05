using Dapr.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApiActor
{
    public class BankActor : Actor, IBankActor, IRemindable
    {
        private BankService _bankService = new BankService();

        public BankActor(ActorHost host)
            : base(host)
        {
        }

        public async Task<AccountBalance> GetAccountBalance()
        {
            var starting = new AccountBalance()
            {
                AccountId = Guid.Parse(Id.GetId()),
                Balance = 100m,
            };

            var balance = await StateManager.GetOrAddStateAsync(nameof(AccountBalance), starting);
            return balance;
        }

        public async Task Withdraw(decimal amount)
        {
            var balance = await GetAccountBalance();

            try
            {
                var updated = _bankService.Withdraw(balance.Balance, amount);

                balance.Balance = updated;

                await StateManager.SetStateAsync(nameof(AccountBalance), balance);
            }
            catch (Exception)
            {
                await RegisterReminderAsync("AccountBalanceNotEnough.", null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
                balance.ReminderCount++;
                await StateManager.SetStateAsync(nameof(AccountBalance), balance);
                throw;
            }
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            var balance = await StateManager.GetStateAsync<AccountBalance>(nameof(AccountBalance));

            Console.WriteLine($"[ReceiveReminderAsync]: name is {reminderName}, count is {balance.ReminderCount}, balance is {balance.Balance}.");

            if (balance.ReminderCount > 3)
            {
                balance.Balance += 100m;
                balance.ReminderCount = 0;
                await StateManager.SetStateAsync(nameof(AccountBalance), balance);
                Console.WriteLine($"[ReceiveReminderAsync]: auto recharg 100m.");
            }
        }
    }
}
