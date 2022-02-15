using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJournalEntry
{
    interface ITransaction
    {
        void AddNewTransaction();
        void GetDefaultCurrentBudget();
        void GetCurrentBudget(int choice);
    }
}
