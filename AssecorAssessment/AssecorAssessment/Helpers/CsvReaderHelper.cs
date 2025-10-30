using System.Text;

namespace AssecorAssessment.Helpers
{
    public static class CsvReaderHelper
    {
        /// <summary>
        /// Reads a CSV file that may contain broken lines and reconstructs valid rows.
        /// </summary>
        public static IEnumerable<string> ReadLogicalLines(string path)
        {
            var lines = File.ReadAllLines(path);
            var buffer = new StringBuilder();

            foreach (var line in lines)
            {
                // Append line to buffer
                buffer.Append(line.Trim());

                // A valid person line should have at least 3 commas
                int commaCount = buffer.ToString().Count(c => c == ',');

                if (commaCount >= 3)
                {
                    yield return buffer.ToString();
                    buffer.Clear();
                }
                else
                {
                    // Continue reading next line
                    buffer.Append(" ");
                }
            }

            // In case something remains
            if (buffer.Length > 0)
                yield return buffer.ToString();
        }
    }
}