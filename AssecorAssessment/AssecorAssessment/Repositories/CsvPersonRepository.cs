using AssecorAssessment.Helpers;
using AssecorAssessment.Models;
using System.Text.RegularExpressions;

namespace AssecorAssessment.Repositories
{
    /// <summary>
    /// CSV-based implementation of the Repository Pattern.
    /// Reads data at startup and keeps it in memory.
    /// 
    /// Why do i use no external CSV library for this implementation?
    /// - The CSV format is non-standard -> Zip and City in one field
    /// - Simple manual parsing is more flexible and transparent
    /// - No additional dependencies required
    /// </summary>
    public class CsvPersonRepository : IPersonRepository
    {
        private const string DEFAULT_FILE_PATH = "sample-input.csv";

        private readonly List<Person> _persons;
        private readonly object _lock = new object();
        private int _nextId;

        public CsvPersonRepository() : this(DEFAULT_FILE_PATH) { }

        public CsvPersonRepository(string filePath)
        {
            _persons = LoadFromCsv(filePath);
            _nextId = _persons.Any() ? _persons.Max(p => p.Id) + 1 : 1;
        }

        private List<Person> LoadFromCsv(string filePath)
        {
            var persons = new List<Person>();

            if (!File.Exists(filePath))
            {
                // Here i would place a log in production code
                return persons;
            }

            var lines = CsvReaderHelper.ReadLogicalLines(filePath);
            int id = 1;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var parts = line.Split(',')
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .ToArray();

                    if (parts.Length < 4)
                        continue;

                    var lastname = parts[0];
                    var name = parts[1];
                    var zipCodeCity = parts[2];
                    var colorIdStr = parts[3];

                    // Extract ZIP code and city with regex
                    var zipCodeCityMatch = Regex.Match(zipCodeCity, @"^(\d+)\s+(.+)$");
                    var zipcode = zipCodeCityMatch.Success ? zipCodeCityMatch.Groups[1].Value : "";
                    var rawCity = zipCodeCityMatch.Success ? zipCodeCityMatch.Groups[2].Value : zipCodeCity;

                    var city = rawCity.Split(" - ")[0].Trim();

                    // Remove emojies or other non-text characters at the end
                    city = Regex.Replace(city, @"[\p{So}\p{Cn}]+$", "").Trim();

                    if (!int.TryParse(colorIdStr, out int colorId))
                        continue;

                    var person = new Person
                    {
                        Id = id++,
                        Lastname = lastname,
                        Name = name,
                        Zipcode = zipcode,
                        City = city,
                        Color = ColorMapper.GetColorName(colorId) ?? "unknown"
                    };

                    persons.Add(person);
                }
                catch
                {
                    // Here i would place a log in production code
                    continue;
                }
            }

            return persons;
        }

        public IEnumerable<Person> GetAll()
        {
            return _persons.ToList();
        }

        public Person? GetById(int id)
        {
            return _persons.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Person> GetByColor(string color)
        {
            return _persons
                .Where(p => p.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Person Add(Person person)
        {
            // Thread-safe for future extensions
            lock (_lock)
            {
                person.Id = _nextId++;
                _persons.Add(person);
                return person;
            }
        }
    }
}