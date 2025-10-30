namespace AssecorAssessment.Models
{
    /// <summary>
    /// Represents a person with address and color information.
    /// Serves as a Data Transfer Object (DTO) between all layers.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique ID (line number from CSV, starting at 1)
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Zipcode { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;
    }
}