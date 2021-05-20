using System.Collections.Generic;
using System.Linq;

namespace INIParser
{
    public class Section
    {
        /// <summary>
        /// Name of this section
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Size of this section based off of number of entries.
        /// </summary>
        public int Size { get { return _entries == null ? 0 : _entries.Count; } }

        private Dictionary<string, Entry> _entries;

        public Section()
        {
            _entries = new Dictionary<string, Entry>();
        }

        public Entry GetEntry(string label)
        {
            if (_entries.ContainsKey(label) == false)
            {
                return null;
            }

            return _entries[label];
        }

        public void SetEntries(Entry[] entries)
        {
            foreach (Entry entry in entries)
            {
                _entries.Add(entry.Label, entry);
            }
        }

        public void AddEntry(Entry entry)
        {
            _entries.Add(entry.Label, entry);
        }

        public string[] GetAllEntryLabels()
        {
            return _entries.Keys.ToArray();
        }
    }
}

