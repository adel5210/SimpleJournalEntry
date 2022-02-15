using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleJournalEntry
{
    class File : IFile
    {
        public string FilePath { get; set; }

        //Constructor, initialize the FilePath on first run of console
        public File()
        {
            FilePath = getLastFilePath();
        }

        //Constructor, if manually specified file path of the data
        public File(string path)
        {
            FilePath = path;
        }

        // Setter of File Path
        public void SetFile(string path)
        {
            this.FilePath = path;
        }


        //Read every lines of set file, returns as listed string
        public List<string> ReadFile()
        {
            List<string> data = new List<string>();

            if (null == FilePath) return data;
            
            using(var reader = new StreamReader(this.FilePath))
            {
                while (!reader.EndOfStream)
                {
                    data.Add(reader.ReadLine());
                }
            }

            return data;
        }

        //returns N line of string from a file
        public IEnumerable<string> ReadLastFileLines(int lastNLines)
        {

            if (null == FilePath) return null;

            return System.IO.File.ReadLines(this.FilePath).TakeLast(lastNLines);
            
        }

        //Writes To file
        public void WriteToFile(string data, params string[] path)
        {
            string p = path.Length == 0 ? this.FilePath : path[0];
            using(StreamWriter writer = System.IO.File.AppendText(p))
            {
                writer.WriteLine(data);
            }
        }

        //Gets a metadata file for initial setup of application where actual data can read and write
        //Saves the path of the file(csv) 
        //Next run of application will automaticall fetch the path and continues with the application
        public string getLastFilePath()
        {
            string metadataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Metadata.json";

            if (System.IO.File.Exists(metadataPath))
            {
                Metadata metadata = JsonFileReader.Read<Metadata>(metadataPath);
                return metadata.Path;
            }

            Console.Write("Set default file path (CSV format) (ex.RawData)\n> ");
            string p = Console.ReadLine();

            Metadata metadataNew = new Metadata();
            metadataNew.Path = p;

            WriteToFile(JsonSerializer.Serialize(metadataNew), metadataPath);

            return p;
        }

        //Read multiple lines and return Listed strings
        public IEnumerable<string> ReadAllLines()
        {
            if (null == FilePath) return null;

            return System.IO.File.ReadAllLines(this.FilePath);
            
        }
    }
}

// Helper class, converts json string to object
public static class JsonFileReader
{
    public static T Read<T>(string filePath)
    {
        string text = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(text);
    }
}
