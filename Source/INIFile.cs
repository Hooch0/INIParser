using System.Collections.Generic;
using System.Linq;

namespace INIParser
{
    public enum INIType {  VALUE, COMMENT, WHITESPACE }


    public class INIFile
    {
        /* An INI is broken into sections.
         *      A section contains the label of the section, the offset, the size, and the entries for the section
         *      An entry contains the label of the entry, and the value that is stored. 
         * 
         * 
         * In a INI, a sections end is determined either when we reach the end of the file or,
         * when a new section is found.
         * 
         * Sections are marked in BRAKETS
         *      [LABEL]
         * Entries have no WHITESPACES
         *      LABEL=VALUE
         *      
         * Semi-colons ; are used as comments
         */

         //TOOD: Read everything as a string

        private Dictionary<string, Section> _sections;

        public INIFile()
        {
            _sections = new Dictionary<string, Section>();
        }

        public INIFile(Section[] sections)
        {
            _sections = new Dictionary<string, Section>();


            foreach (Section section in sections)
            {
                _sections.Add(section.Label, section);
            }
        }

        /// <summary>
        /// Gets an entry from the INI File.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public Entry GetEntry(string section, string entry)
        {
            if (_sections.ContainsKey(section) == false)
            {
                return null;
            }

            return _sections[section].GetEntry(entry);
        }

        /// <summary>
        /// Gets a section from the INI file.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public Section GetSection(string section)
        {
            if (_sections.ContainsKey(section) == false)
            {
                return null;
            }

            return _sections[section];
        }

        /// <summary>
        /// Gets all section labels.
        /// </summary>
        /// <returns></returns>
        public string[] GetAllSectionLabels()
        {
            return _sections.Keys.ToArray();
        }

        /// <summary>
        /// Gets all entry labels for a specified section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public string[] GetAllEntryLabels(string section)
        {
            if (ContainsSection(section))
            {
                return _sections[section].GetAllEntryLabels();
            }

            return null;
        }

        /// <summary>
        /// Adds a new section.
        /// </summary>
        /// <param name="section"></param>
        public void AddSection(string section)
        {
            if (ContainsSection(section) == false)
            {
                _sections.Add(section, new Section() { Label = section });
            }
        }

        /// <summary>
        /// Adds a new entry to a specified section.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entryName"></param>
        /// <param name="entryValue"></param>
        /// <param name="type"></param>
        public void AddEntry(string section, string entryName, string entryValue, INIType type)
        {
            if (ContainsEntry(section, entryName) == false) 
            {
                _sections[section].AddEntry(new Entry(entryName, entryValue, type));
            }
        }

        /// <summary>
        /// Checks if a specific section exist
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public bool ContainsSection(string section)
        {
            return _sections.ContainsKey(section);
        }

        /// <summary>
        /// Checks if a specific entry in a specified section exist
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool ContainsEntry(string section, string entry)
        {
            return ContainsSection(section) && _sections[section].GetEntry(entry) != null;
        }

        public override string ToString()
        {
            string val = "";

            foreach(string section in _sections.Keys)
            {
                val += "[" +_sections[section].Label + "]\n";

                foreach(string entryLabel in _sections[section].GetAllEntryLabels())
                {
                    Entry entry = _sections[section].GetEntry(entryLabel);
                    if (entry.Type == INIType.VALUE)
                    {
                        val += entry.Label + "=" + entry.Value.ToString();
                    }
                    else
                    {
                        val += entry.Value.ToString();
                    }
                    
                }
            }

            return val;
        }

    }
}
