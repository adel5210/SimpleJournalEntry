using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJournalEntry
{
    class Transaction : ITransaction
    {

        public IFile _File { get; set; }
        public double TotolBudget { get; set; } = 0;

        public Transaction(IFile file)
        {
            this._File = file;
        }

        // Creates new transaction
        public void AddNewTransaction()
        {
            Console.WriteLine("\nAdd Record");

            Console.Write(" - Amount: ");
            string amount = Console.ReadLine();

            Console.Write(" - Date (MM/DD/YYYY): ");
            string date = Console.ReadLine();

            Console.Write(" - Description: ");
            string description = Console.ReadLine();

            if (process(amount, date, description))
            {
                _File.WriteToFile(formatDataToCSV(amount, description, date));
            }
        }

        //Validation process, return true if inputs is valid
        private bool process(string? amount, string? date, string? description)
        {
            Tuple<double, bool> _amount = StringToDouble(amount, "Amount");
            Tuple<DateTime, bool> _date = StringToDate(date, "Date");


            if(_amount.Item2 || _date.Item2)
            {
                Console.WriteLine("Transaction cannot be process");
                return false;
            }

            Console.WriteLine("Transaction can be process");
            return true;

        }


        // Gets default output for curent budget and last 10 transactions
        public void GetDefaultCurrentBudget()
        {
            Console.WriteLine("\n========================================================================");

            //Last 10 transactions
            GetLastNTxns(10);

            //Current balance
            var listesAmounts = from txn in _File.ReadAllLines()
                                  select (StringToDouble(txn.Split(",")[0],"").Item1);
            
            Console.Write("\nCurrent balance: "+ listesAmounts.Sum(s => s));
            Console.WriteLine("\n========================================================================");
        }

        //Helper function, get last n transaction 
        private void GetLastNTxns(int n)
        {
            
            Console.WriteLine("Last "+n+" transactions");
            Console.WriteLine("Amount\t\t|Date\t\t|Description");
            foreach (string txn in _File.ReadLastFileLines(n))
            {
                string[] txnEachParam = txn.Split(',');
                Console.WriteLine(txnEachParam[0] + "\t\t|" + txnEachParam[1] + "\t|" + txnEachParam[2]);
            }
        }

        //Buget menu, user input selection from second menu
        public void GetCurrentBudget(int choice)
        {
            switch (choice)
            {
                // 
                case 1:
                    Console.Write("How many last N transaction? \n> ");
                    int val = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("\n========================================================================");
                    GetLastNTxns(val);
                    Console.WriteLine("\n========================================================================");
                    break;
                case 2:
                    Console.Write("Specify Date (MM/DD/YYYY) \n> ");
                    string stringDate = Console.ReadLine();
                    var date = StringToDate(stringDate, "Date");
                    if (!date.Item2)
                    {
                        var listedDates = from txn in _File.ReadAllLines()
                                            where (txn.Split(",")[1]).Equals(stringDate)
                                            select txn;
                        Console.WriteLine("\n========================================================================");
                        foreach (var txn in listedDates)
                        {
                            string[] txnEachParam = txn.Split(',');
                            Console.WriteLine(txnEachParam[0] + "\t\t|" + txnEachParam[1] + "\t|" + txnEachParam[2]);
                        }
                        Console.WriteLine("\n========================================================================");
                    }
                    break;
            }
        }   

        //Helper functions, formats the output to process to a csv file
        string formatDataToCSV(string amount, string description, string date)
        {
            return amount + "," + date + "," +'"'+description + '"';
        }

        //convert string to double, returns the type with exception flag occurance
        Tuple<double,bool> StringToDouble(string val, string param)
        {
            double v = 0;
            bool exceptionFlag = false;
            try
            {
                v = Convert.ToDouble(val);
            }
            catch
            {
                InvalidDataType(param);
                exceptionFlag = true;
            }
            return new Tuple<double, bool>(v, exceptionFlag);
        }


        //convert string to date, returns the type with exception flag occurance
        private Tuple<DateTime, bool> StringToDate(string date, string param)
        {
            DateTime v = new DateTime();
            bool exceptionFlag = false;
            try
            {
                v = Convert.ToDateTime(date);
            }
            catch
            {
                InvalidDataType(param);
                exceptionFlag = true;
            }
            return new Tuple<DateTime, bool>(v, exceptionFlag);
        }

        // displays message invalid data type
        void InvalidDataType(string param)
        {
            Console.WriteLine("Exception: Invalid data type for " + param);
        }
    }
}
