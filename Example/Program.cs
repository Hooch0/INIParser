using System;
using System.IO;
using INIParser;

namespace INI_Parser
{
    class Program
    {
        private static INIFile _file;

        static void Main(string[] args)
        {
            ReadINI();
        }

        private static void WriteINI()
        {
            _file = new INIFile();
            _file.AddSection("Input");
            _file.AddEntry("Input", "Shoot", "mouse1", INIParser.INIType.VALUE);
            _file.AddEntry("Input", "Automatic", false.ToString(), INIParser.INIType.VALUE);
            _file.AddEntry("Input", "comment1","This bool defines if the current weapon is fully automatic or single shot", INIParser.INIType.COMMENT);

            Console.WriteLine("Saving file to..." + System.IO.Directory.GetCurrentDirectory());
            INIWriter writer = new INIWriter();

            using (FileStream s = File.Create(System.IO.Directory.GetCurrentDirectory() + "/test.ini"))
            { 
                writer.Write(_file, s);
            }
        }

        private static void ReadINI()
        {
            INIReader reader = new INIReader();
            using (FileStream s = File.OpenRead(System.IO.Directory.GetCurrentDirectory() + "/test.ini"))
            {
                _file = reader.Read(s);
            }

            Console.WriteLine("Attempting to read INI...");
            Console.WriteLine(_file.ToString());
        }
    }
}
