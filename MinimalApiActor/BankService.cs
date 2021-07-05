using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApiActor
{
    public class BankService
    {
        private readonly decimal _overdraftThreshold = -50m;

        public decimal Withdraw(decimal balance, decimal amount)
        {
            var updated = balance - amount;
            if (updated < _overdraftThreshold)
            {
                throw new Exception($"Insufficient Balance");
            }

            return updated;
        }
    }
}
