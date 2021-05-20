using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

namespace INIParser
{
    public class INIReader
    {

        private List<Section> _sections;


        public INIFile Read(Stream stream)
        {
            _sections = new List<Section>();
            return new INIFile(ReadSections(stream));
        }

        private Section[] ReadSections(Stream stream)
        {

            Section section = null;

            while (stream.Position < stream.Length)
            {
                section = ReadSection(stream);

                if (section != null)
                {
                    _sections.Add(section);
                }
            }


            _sections.Reverse();
            return _sections.ToArray();

        }

        private Section ReadSection(Stream stream, string existingLine = "")
        {


            //Get the section start
            string line = "";

            if (existingLine != "")
            {
                line = existingLine;
            }
            else
            {
                line = ReadLine(stream);
            }



            if ((line.Contains("[") && line.Contains("]")) == true)
            {
                Section section = new Section();

                //the label does not contain the brackets
                section.Label = line.Substring(1, line.IndexOf("]") - 1);
                section.SetEntries(ReadEntries(stream));

                return section;

            }


            return null;


        }

        private Entry[] ReadEntries(Stream stream)
        {
            List<Entry> entries = new List<Entry>();


            string line = "";

            while ((line.Contains("[") && line.Contains("]")) == false && stream.Position < stream.Length)
            {
                
                if (line != "")
                {
                    entries.Add(ReadEntry(stream, line));
                }
                line = ReadLine(stream);
            }

            //we have encountered a new section
            if (line.Contains("[") && line.Contains("]"))
            {
                _sections.Add(ReadSection(stream, line));
            }
            else if (string.IsNullOrWhiteSpace(line) == false)
            {
                entries.Add(ReadEntry(stream, line));
            }

            return entries.ToArray();

        }

        private Entry ReadEntry(Stream stream, string line)
        {
           

            if (string.IsNullOrWhiteSpace(line))
            {
                //Whitespace entry
                return new Entry("","",INIType.WHITESPACE);
            }

            if (line.Length > 0 && line[0] == ';')
            {
                //Comment entry
                return new Entry("",line, INIType.COMMENT);
            }
            //Value entry
            string label = line.Substring(0, line.IndexOf('='));
            string value = value = line.Substring(line.IndexOf('=') + 1);;
            INIType type = INIType.VALUE;


            return new Entry(label, value, type);
        }

        private string ReadLine(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            bool end = false;


            while (stream.CanRead == true && end == false)
            {
                char c = ReadByte(stream);

                end = (c == '\n');
                sb.Append(c);
            }

            return sb.ToString();
        }

        private char ReadByte(Stream stream)
        {
            return Encoding.ASCII.GetChars(new[] { (byte)stream.ReadByte() })[0];
        }
    }
}

