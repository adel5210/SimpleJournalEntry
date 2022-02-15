using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJournalEntry
{
    class Selection
    {
        /**
         * Dependency Injection
         */
        public IFile _File { get; set; }
        public ITransaction _Transaction { get; set; }


        //Displays first menu with user choice
        public void Init()
        {
            
            _File = new File();
            _Transaction = new Transaction(_File);

            while (true)
            {

                Console.WriteLine("\n==== Simple Journal Entry Menu ====");
                Console.WriteLine("Please insert a key number provide a selection: ");
                Console.WriteLine("1. Record a transaction");
                Console.WriteLine("2. Fetch current budget");
                Console.WriteLine("3. Exit the program");
                Console.Write("> ");
                int selectedValue = GetInputForMenu();

                switch (selectedValue)
                {
                    case 1:
                        RecordTransaction();
                        break;
                    case 2:
                        CalculateBudget();
                        break;
                    case 3:
                        Exit();
                        break;
                }
            }
        }   

        //Helper call, returns a right user selection or throws an "Invalid input" message  
        private int GetInputForMenu()
        {
            int selectedValue;
            List<int> validVal = new() { 1, 2, 3 };

            while (true)
            {
                string input = Console.ReadLine();
                
                try
                {
                    selectedValue = Convert.ToInt32(input);


                    if (!validVal.Contains(selectedValue))
                    {
                        InvalidInput();
                        continue;
                    }

                    return selectedValue;
                }
                catch
                {
                    InvalidInput();
                }
            }

        }

        //Displays the "Invalid Input" message
        private void InvalidInput()
        {
            Console.Write("--- Invalid Input, try again --- \n> ");
        }

        // Calls the process for creating a transaction from first menu , user input 1
        public void RecordTransaction()
        {
            string input = "";

            while (!input.Equals("n"))
            {
                _Transaction.AddNewTransaction();

                Console.Write("Add another record? (y/n) \n> ");
                input = Console.ReadLine();
            }

        }

        // Calls the process for computing the overall budget with the second menu "Budget menu" , user input 2 from first menu
        public void CalculateBudget()
        {

            _Transaction.GetDefaultCurrentBudget();

            int input = int.MinValue;

            while (!input.Equals(3) || input.Equals(int.MinValue))
            {
                Console.WriteLine("\n==== Budget Menu ====");
                Console.WriteLine("1. Fetch last N transactions ");
                Console.WriteLine("2. Fetch transactions by date ");
                Console.Write("3. Return to menu \n> ");
                input = GetInputForMenu();
                _Transaction.GetCurrentBudget(input);
            }
        }

        //Exits the application when user inputs 3 from first menu
        void Exit()
        {
            Environment.Exit(0);
        }

    }
}
