using System;

namespace INIParser
{
    public class Entry
    {
        private static int _whitespaceCount;
        private static int _commentCount;

        /// <summary>
        /// What the entry is called.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// The value stored in the entry.
        /// </summary>
        public string Value { get; private set; }

        public INIType Type { get; private set; }

        public Entry(string label, string value, INIType type)
        {
            string newLabel = label;

            if (type == INIType.WHITESPACE)
            {
                newLabel = label + _whitespaceCount.ToString();
                _whitespaceCount++;
            }
            else if (type == INIType.COMMENT)
            {
                newLabel = label + _commentCount.ToString();
                _commentCount++;
            }

            Label = newLabel;
            Value = value;
            Type = type;
        }

        ~Entry()
        {
            _whitespaceCount--;
            _commentCount--;
        }


        /// <summary>
        /// Parsed Value to the type T
        /// </summary>
        /// <typeparam name="T">Type to parse too</typeparam>
        /// <returns>Value parsed</returns>
        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }

        /// <summary>
        /// Attempts to parse Value to type T
        /// </summary>
        /// <typeparam name="T">Type to parse Value to</typeparam>
        /// <param name="value">Value to contain parsed Value</param>
        /// <returns>True if parse was succesful, false otherwise</returns>
        public bool TryParseValue<T>(out T value)
        {

            bool success = false;
            T convertedVal;
            try
            {
                convertedVal = (T)Convert.ChangeType(Value, typeof(T));
                success = true;
            }
            catch(Exception)
            {
                value = default(T);
                return false;
            }
            value = convertedVal;
            return success;
        }
    }
}
