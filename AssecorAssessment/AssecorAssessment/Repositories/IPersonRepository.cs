using AssecorAssessment.Models;

namespace AssecorAssessment.Repositories
{
    /// <summary>
    /// Repository pattern interface for data access to persons.
    /// Abstracts the data source and allows easy replacement.
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Returns all persons.
        /// </summary>
        IEnumerable<Person> GetAll();

        /// <summary>
        /// Finds a person by their ID.
        /// </summary>
        /// <param name="id">The ID of the person (row number).</param>
        /// <returns>Person or null if not found.</returns>
        Person? GetById(int id);

        /// <summary>
        /// Filters persons by their favorite color.
        /// </summary>
        /// <param name="color">The color (case-insensitive).</param>
        IEnumerable<Person> GetByColor(string color);

        /// <summary>
        /// Adds a new person (bonus feature).
        /// </summary>
        /// <param name="person">The person to add.</param>
        /// <returns>The person with an assigned ID.</returns>
        Person Add(Person person);
    }
}