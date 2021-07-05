using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApiActor
{
    public class AccountBalance
    {
        public Guid AccountId { get; set; }

        public decimal Balance { get; set; }

        public int ReminderCount { get; set; }
    }
}
