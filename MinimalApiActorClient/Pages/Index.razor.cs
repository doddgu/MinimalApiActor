using Dapr.Actors.Client;
using Dapr.Actors;
using MinimalApiActor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApiActorClient.Pages
{
    public partial class Index
    {
        private async Task TestActor()
        {
            var bank = ActorProxy.Create<IBankActor>(ActorId.CreateRandom(), "BankActor");

            Enumerable.Range(0, 2).AsParallel().ForAll(async times =>
            {
                var balance = await bank.GetAccountBalance();
                Console.WriteLine($"Balance for account '{balance.AccountId}' is '{balance.Balance:c}'.");

                Console.WriteLine($"Withdrawing '{10m:c}'...");
                try
                {
                    await bank.Withdraw(10m);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Overdraft: " + ex.Message);
                }
            });

            await Task.CompletedTask;
        }
    }
}
