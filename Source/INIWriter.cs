using System;
using System.IO;
using System.Text;

namespace INIParser
{
    public class INIWriter
    {
        /* Does require an File premade.
         * Should be possible to create one through code and enter 
         * each entry manually.
         */

        public void Write(INIFile file, Stream stream)
        {
            string[] sections = file.GetAllSectionLabels();
            foreach (string section in sections)
            {
                WriteSection(stream, file.GetSection(section));
            }
        }

        private void WriteSection(Stream stream, Section section)
        {
            WriteString(stream, "[" + section.Label + "]");
            WriteNewLine(stream);

            string[] entries = section.GetAllEntryLabels();

            for (int i = 0; i < section.Size; i++)
            {
                WriteEntry(stream, section.GetEntry(entries[i]));
            }

        }

        private void WriteEntry(Stream stream, Entry entry)
        {
            string line = "";
            bool newLine = true;
           
            if (entry.Type == INIType.WHITESPACE)
            {
                WriteNewLine(stream);
                return;
            }
            else if (entry.Type == INIType.COMMENT)
            {
                string comment = "";

                if (entry.Value.ToString().StartsWith(";") == false)
                {
                    comment = ";";

                }


                comment += entry.Value.ToString();

                line = comment;
                newLine = comment.Contains(Environment.NewLine) == false;

            }
            else
            {

                //any other value type will be writen as a string
                string val = entry.Value.ToString();
                newLine = val.Contains(Environment.NewLine) == false;

                line = entry.Label + "=" + entry.Value.ToString();
            }

            WriteString(stream, line);

            if (newLine == true)
            {
                WriteNewLine(stream);
            }

        }

        private void WriteNewLine(Stream stream)
        {
            WriteString(stream, Environment.NewLine);
        }

        private void WriteBytes(Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        private void WriteString(Stream stream, string val)
        {
            WriteBytes(stream, Encoding.ASCII.GetBytes(val));
        }

    }
}


