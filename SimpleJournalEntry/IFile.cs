using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJournalEntry
{
    interface IFile
    {
        void SetFile(string path);
        List<string> ReadFile();
        IEnumerable<string> ReadLastFileLines(int lines);
        IEnumerable<string> ReadAllLines();
        void WriteToFile(string data, params string[] path);
        string getLastFilePath(); 
    }
}
